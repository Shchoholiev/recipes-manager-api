using Azure.AI.OpenAI;
using RecipesManagerApi.Application.Models.OpenAi;

namespace RecipesManagerApi.Application.IServices;

public interface IOpenAiService
{
    Task<OpenAiResponse?> GetChatCompletion(ChatCompletionRequest chat, CancellationToken cancellationToken);

    IAsyncEnumerable<OpenAiResponse> GetChatCompletionStream(ChatCompletionsOptions chat, CancellationToken cancellationToken);
}
