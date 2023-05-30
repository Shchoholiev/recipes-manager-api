using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipesManagerApi.Api.Models;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Api.Controllers;

[Authorize]
public class IngredientsController : ApiController
{
    private readonly IIngredientsService _ingredientsService;

    public IngredientsController(IIngredientsService ingredientsService)
    {
        _ingredientsService = ingredientsService;
    }

    [HttpPost("parse")]
    public async Task ParseIngredientsAsync([FromBody] IngredientsParseInput input, CancellationToken cancellationToken) {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");
        await Response.Body.FlushAsync(cancellationToken);

        var ingredients = _ingredientsService.ParseIngredientsAsync(input.Text, cancellationToken);

        await foreach (var ingredient in ingredients)
        {
            var chunk = JsonConvert.SerializeObject(ingredient);
            await Response.WriteAsync($"data: {chunk}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }

    [HttpPost("estimate-calories")]
    public async Task EstimateCaloriesAsync([FromBody] List<IngredientDto> ingredientsDtos, CancellationToken cancellationToken) {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");
        await Response.Body.FlushAsync(cancellationToken);

        var ingredients = _ingredientsService.EstimateIngredientsCaloriesAsync(ingredientsDtos, cancellationToken);
        
        await foreach (var ingredient in ingredients)
        {
            var chunk = JsonConvert.SerializeObject(ingredient);
            await Response.WriteAsync($"data: {chunk}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }
}
