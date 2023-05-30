using System.Text;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
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

    private readonly IConfiguration _configuration;

    private readonly OpenAIClient _openAIClient;

    public OpenAiService(IHttpClientFactory httpClientFactory, IOpenAiLogsRepository openAiLogsRepository, IOpenAiLogsService openAiLogsService, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient("OpenAiHttpClient");
        _openAiLogsRepository = openAiLogsRepository;
        _openAiLogsService = openAiLogsService;
        _configuration = configuration;

	    var openAiApiKey = _configuration.GetSection("OpenAi")?.GetValue<string>("ApiKey");
        _openAIClient = new OpenAIClient(openAiApiKey, new OpenAIClientOptions());
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

    public async IAsyncEnumerable<OpenAiResponse> GetChatCompletionStream(ChatCompletionsOptions chat, CancellationToken cancellationToken)
    {
        var jsonBody = JsonConvert.SerializeObject(chat, _jsonSettings);
        var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        Console.WriteLine(jsonBody);
        var log = await _openAiLogsService.AddLogAsync(new OpenAiLogDto {
            Request = jsonBody,
            Response = null,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedById = GlobalUser.Id.ToString() ?? ObjectId.Empty.ToString(),
        }, cancellationToken);

        var allData = string.Empty;

        var response = await _openAIClient.GetChatCompletionsStreamingAsync(
            deploymentOrModelName: "gpt-3.5-turbo",
            chat,
            cancellationToken);
        using StreamingChatCompletions streamingChatCompletions = response.Value;

        await foreach (var choice in streamingChatCompletions.GetChoicesStreaming(cancellationToken))
        {
            await foreach (var message in choice.GetMessageStreaming(cancellationToken))
            {
                allData += JsonConvert.SerializeObject(message)+ "\n\n";;
                if (message.Content == null) {
                    log.Response = allData;
                    Task.Run(() => _openAiLogsService.UpdateLogAsync(log, cancellationToken));
                }

                var openAiResponse = new OpenAiResponse {
                    Choices = new List<OpenAiChoice> { new OpenAiChoice { Delta = new OpenAiDelta { Content = message.Content } }}
                };

                yield return openAiResponse;
            }
        }
    }
}
