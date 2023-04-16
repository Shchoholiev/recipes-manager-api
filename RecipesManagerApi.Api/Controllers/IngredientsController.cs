using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using RecipesManagerApi.Api.Models;
using RecipesManagerApi.Application.IServices;

namespace RecipesManagerApi.Api.Controllers;

[Route("ingredients")]
public class IngredientsController : Controller
{
    private readonly IIngredientsService _ingredientsService;

    public IngredientsController(IIngredientsService ingredientsService)
    {
        _ingredientsService = ingredientsService;
    }

    [HttpPost("parse")]
    public async Task ParseingredientsAsync([FromBody] IngredientsParseInput input, CancellationToken cancellationToken) {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");

        var ingredients = _ingredientsService.ParseIngredientsAsync(input.Text, cancellationToken);
        var writer = new HttpResponseStreamWriter(Response.Body, Encoding.UTF8);

        await foreach (var ingredient in ingredients)
        {
            var chunk = JsonConvert.SerializeObject(ingredient);
            var hexadecimalLength = Encoding.UTF8.GetByteCount(chunk).ToString("X");
            await writer.WriteAsync($"{hexadecimalLength}\r\n{chunk}\r\n");
            await writer.FlushAsync();
        }
        await writer.WriteAsync("0\r\n\r\n");
        await writer.FlushAsync();
    }
}