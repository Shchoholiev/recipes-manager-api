namespace RecipesManagerApi.Application.Models.OpenAi;

public class OpenAiUsage
{
    public int PromptTokens { get; set; }

    public int CompletionTokens { get; set; }

    public int TotalTokens { get; set; }
}
