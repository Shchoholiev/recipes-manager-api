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

        var httpResponse = await _httpClient.PostAsync("chat/completions", body, cancellationToken);
        var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        await _openAiLogsRepository.AddAsync(new OpenAiLog {
            Request = jsonBody,
            Response = responseBody,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedById = GlobalUser.Id ?? ObjectId.Empty,
        }, cancellationToken);
    
        var response = JsonConvert.DeserializeObject<OpenAiResponse>(responseBody, _jsonSettings);

        return response;
    }

    public async Task<Stream> GetChatCompletionStream(ChatCompletionRequest chat, CancellationToken cancellationToken)
    {
        chat.Stream = true;
        var jsonBody = JsonConvert.SerializeObject(chat, _jsonSettings);
        var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var httpResponse = await _httpClient.PostAsync("chat/completions", body, cancellationToken);
        await _openAiLogsRepository.AddAsync(new OpenAiLog {
            Request = jsonBody,
            Response = "Stream",
            CreatedDateUtc = DateTime.UtcNow,
            CreatedById = GlobalUser.Id ?? ObjectId.Empty,
        }, cancellationToken);
        var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

        return stream;
    }
}
