namespace RecipesManagerApi.Application.Models.OpenAi;

public class OpenAiChoice
{
    public OpenAiMessage Message { get; set; }

    public string FinishReason { get; set; }
    
    public int Index { get; set; }
}
