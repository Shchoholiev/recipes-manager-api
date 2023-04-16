using System.Text;
using Newtonsoft.Json;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.OpenAi;

namespace RecipesManagerApi.Infrastructure.Services;

public class IngredientsService : IIngredientsService
{
	private readonly IOpenAiService _openAiService;

	public IngredientsService(IOpenAiService openAiService)
	{
		_openAiService = openAiService;
	}

	public async IAsyncEnumerable<IngredientDto> ParseIngredientsAsync(string text, CancellationToken cancellationToken) {
		var chat = new ChatCompletionRequest {
			MaxTokens = 1024,
			Messages = new List<OpenAiMessage> {
				new OpenAiMessage {
					Role = "system",
					Content = $"You are an ingredients parser"
				},
				new OpenAiMessage {
					Role = "user",
					Content = $"Parse ingredients into a JSON format. Ingredient has name, units and amount. Amount must be double. If value is missing use null. Return array.\n\n " + 
						$"Ingredients:\n {text}"
				}
			}
		};

		var responses = _openAiService.GetChatCompletionStream(chat, cancellationToken);
		var json = new StringBuilder();
		await foreach (var response in responses)
		{
			var chunk = response.Choices.FirstOrDefault()?.Delta?.Content;
			var formatted = chunk?.Replace("\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
			json.Append(formatted);
			if (formatted != null && formatted.Contains("}"))
			{
				var ingredientJson = json.ToString().Trim().TrimEnd(',');
				var ingredient = JsonConvert.DeserializeObject<IngredientDto>(ingredientJson);
				json.Clear();

				yield return ingredient;
			}
		}
	}
}
