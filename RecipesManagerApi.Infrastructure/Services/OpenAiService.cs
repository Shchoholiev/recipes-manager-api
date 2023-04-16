using System.Text;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RecipesManagerApi.Application.GlodalInstances;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.OpenAi;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;

public class OpenAiService : IOpenAiService
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    private readonly IOpenAiLogsRepository _openAiLogsRepository;

    public OpenAiService(IHttpClientFactory httpClientFactory, IOpenAiLogsRepository openAiLogsRepository)
    {
        _httpClient = httpClientFactory.CreateClient("OpenAiHttpClient");
        _openAiLogsRepository = openAiLogsRepository;
    }

    public async Task<OpenAiResponse?> GetChatCompletion(ChatCompletionRequest chat, CancellationToken cancellationToken)
    {
        chat.Stream = false;
        var jsonBody = JsonConvert.SerializeObject(chat, _jsonSettings);
        var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var log = await _openAiLogsRepository.AddAsync(new OpenAiLog {
            Request = jsonBody,
            Response = null,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedById = GlobalUser.Id ?? ObjectId.Empty,
        }, cancellationToken);

        var httpResponse = await _httpClient.PostAsync("chat/completions", body, cancellationToken);
        var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        log.Response = responseBody;
        Task.Run(() => _openAiLogsRepository.UpdateAsync(log, cancellationToken));
    
        var response = JsonConvert.DeserializeObject<OpenAiResponse>(responseBody, _jsonSettings);

        return response;
    }

    public async IAsyncEnumerable<OpenAiResponse> GetChatCompletionStream(ChatCompletionRequest chat, CancellationToken cancellationToken)
    {
        chat.Stream = true;
        var jsonBody = JsonConvert.SerializeObject(chat, _jsonSettings);
        var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var log = await _openAiLogsRepository.AddAsync(new OpenAiLog {
            Request = jsonBody,
            Response = null,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedById = GlobalUser.Id ?? ObjectId.Empty,
        }, cancellationToken);

        var httpResponse = await _httpClient.PostAsync("chat/completions", body, cancellationToken);
        var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

        var allData = string.Empty;
        var lastIndex = 0;

        var buffer = new byte[256];
		var bytesRead = 0;
		while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
		{
			var chunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            allData += chunk;

            var endIndex = allData.IndexOf("\n\n", lastIndex);
            while (endIndex > -1)
            {
                var json = allData.Substring(lastIndex + 6, endIndex - lastIndex - 6);
                if (json == "[DONE]") {
                    endIndex = -1;
                    continue;
                }
                var openAiResponse = JsonConvert.DeserializeObject<OpenAiResponse>(json, _jsonSettings);
                yield return openAiResponse;

                lastIndex = endIndex + 2;
                endIndex = allData.IndexOf("\n\n", lastIndex);
            }
		}

        log.Response = allData;
        Task.Run(() => _openAiLogsRepository.UpdateAsync(log, cancellationToken));
    }
}
