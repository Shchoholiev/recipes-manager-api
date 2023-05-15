using System.Text;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RecipesManagerApi.Application.GlodalInstances;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
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

    private readonly IOpenAiLogsService _openAiLogsService;

    public OpenAiService(IHttpClientFactory httpClientFactory, IOpenAiLogsRepository openAiLogsRepository, IOpenAiLogsService openAiLogsService)
    {
        _httpClient = httpClientFactory.CreateClient("OpenAiHttpClient");
        _openAiLogsRepository = openAiLogsRepository;
        _openAiLogsService = openAiLogsService;
    }

    public async Task<OpenAiResponse?> GetChatCompletion(ChatCompletionRequest chat, CancellationToken cancellationToken)
    {
        chat.Stream = false;
        var jsonBody = JsonConvert.SerializeObject(chat, _jsonSettings);
        var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var log = await _openAiLogsService.AddLogAsync(new OpenAiLogDto {
            Request = jsonBody,
            Response = null,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedById = GlobalUser.Id.ToString() ?? ObjectId.Empty.ToString(),
        }, cancellationToken);

        var httpResponse = await _httpClient.PostAsync("chat/completions", body, cancellationToken);
        var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        log.Response = responseBody;
        Task.Run(async () => _openAiLogsService.UpdateLogAsync(log, cancellationToken));
    
        var response = JsonConvert.DeserializeObject<OpenAiResponse>(responseBody, _jsonSettings);

        return response;
    }

    public async IAsyncEnumerable<OpenAiResponse> GetChatCompletionStream(ChatCompletionRequest chat, CancellationToken cancellationToken)
    {
        chat.Stream = true;
        var jsonBody = JsonConvert.SerializeObject(chat, _jsonSettings);
        var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var log = await _openAiLogsService.AddLogAsync(new OpenAiLogDto {
            Request = jsonBody,
            Response = null,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedById = GlobalUser.Id.ToString() ?? ObjectId.Empty.ToString(),
        }, cancellationToken);

        var httpResponse = await _httpClient.PostAsync("chat/completions", body, cancellationToken);

        var allData = string.Empty;
        using var streamReader = new StreamReader(await httpResponse.Content.ReadAsStreamAsync(cancellationToken));
        while (!streamReader.EndOfStream)
        {
            var line = await streamReader.ReadLineAsync();
            allData += line + "\n\n";
            if (string.IsNullOrEmpty(line)) continue;
            
            var json = line?.Substring(6, line.Length - 6);
            if (json == "[DONE]") yield break;

            var openAiResponse = JsonConvert.DeserializeObject<OpenAiResponse>(json, _jsonSettings);
            yield return openAiResponse;
        }
        
        log.Response = allData;
        Task.Run(() => _openAiLogsService.UpdateLogAsync(log, cancellationToken));
    }
}
