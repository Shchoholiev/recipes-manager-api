using System.Text;
using AutoMapper;
using Azure.AI.OpenAI;
using Newtonsoft.Json;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.OpenAi;
using RecipesManagerApi.Application.Models.ShortDtos;

namespace RecipesManagerApi.Infrastructure.Services;

public class IngredientsService : IIngredientsService
{
	private readonly IOpenAiService _openAiService;

	private readonly IMapper _mapper;

	public IngredientsService(
		IOpenAiService openAiService,
		IMapper mapper)
	{
		_openAiService = openAiService;
		_mapper = mapper;
	}

	public async IAsyncEnumerable<IngredientDto> ParseIngredientsAsync(string text, CancellationToken cancellationToken) {
		var chat = new ChatCompletionsOptions {
			Messages = {
				new ChatMessage (
					ChatRole.System,
					"You are an ingredients parser"
				),
				new ChatMessage (
					ChatRole.User,
					$"Parse ingredients into a JSON format. Ingredient has name, units and amount. Amount must be double. If value is missing use null. Return array.\n\n " + 
					$"Ingredients:\n {text} \n\nThe JSON response:"
				)
			},
			MaxTokens = 1024
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

	public async IAsyncEnumerable<IngredientDto> EstimateIngredientsCaloriesAsync(List<IngredientDto> ingredients, CancellationToken cancellationToken) {
		var inputIngredients = _mapper.Map<List<IngredientShortDto>>(ingredients);
		var ingredientsJson = JsonConvert.SerializeObject(ingredients, new JsonSerializerSettings { Formatting = Formatting.None });
		ingredientsJson = ingredientsJson.Replace("},", "},\n");
		var chat = new ChatCompletionsOptions {
			Messages = {
				new ChatMessage (
					ChatRole.System,
					"You are calories estimator"
				),
				new ChatMessage (
					ChatRole.User,
					$"Estimate calories per unit of ingredient. Add CaloriesPerUnit property to json. CaloriesPerUnit must be integer. Return array.\n\n " + 
					$"Ingredients:\n {ingredientsJson} \n\nThe JSON response:"
				)
			},
			MaxTokens = 1024
		};

		var responses = _openAiService.GetChatCompletionStream(chat, cancellationToken);
		var json = new StringBuilder();
		var index = 0;
		await foreach (var response in responses)
		{
			var chunk = response.Choices.FirstOrDefault()?.Delta?.Content;
			var formatted = chunk?.Replace("\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
			json.Append(formatted);
			if (formatted != null && formatted.Contains("}"))
			{
				var ingredientJson = json.ToString().Trim().TrimEnd(',');
				var newIngredient = JsonConvert.DeserializeObject<IngredientDto>(ingredientJson);
				json.Clear();
				var ingredient = ingredients[index];
				ingredient.CaloriesPerUnit = newIngredient?.CaloriesPerUnit ?? 0;
				ingredient.TotalCalories = (int)(ingredient.CaloriesPerUnit * ingredient.Amount ?? 0);
				index++;

				yield return ingredient;
			}
		}
	}
}
