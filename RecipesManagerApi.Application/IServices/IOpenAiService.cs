using RecipesManagerApi.Application.Models.OpenAi;

namespace RecipesManagerApi.Application.IServices;

public interface IOpenAiService
{
    Task<OpenAiResponse?> GetChatCompletion(ChatCompletionRequest chat, CancellationToken cancellationToken);

    Task<Stream> GetChatCompletionStream(ChatCompletionRequest chat, CancellationToken cancellationToken);
}
