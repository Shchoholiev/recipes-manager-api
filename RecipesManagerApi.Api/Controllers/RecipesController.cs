using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Api.Controllers;

[Authorize]
public class RecipesController : ApiController
{
    private readonly ILogger<RecipesController> _logger;

    private readonly IRecipesService _recipesService;

    public RecipesController(
        ILogger<RecipesController> logger,
        IRecipesService recipesService)
    {
        _logger = logger;
        _recipesService = recipesService;
    }

    [HttpPost]  
    public async Task<IActionResult> CreateRecipeAsync([FromForm]RecipeCreateDto dto, CancellationToken cancellationToken)
    {
        var recipe = await _recipesService.AddRecipeAsync(dto, cancellationToken);
        return CreatedAtAction(null, new { id = recipe.Id }, recipe);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipeAsync(string id, [FromForm] RecipeCreateDto dto, CancellationToken cancellationToken)
    {
        var recipe = await _recipesService.UpdateRecipeAsync(id, dto, cancellationToken);
        return Ok(recipe);
    }
}
