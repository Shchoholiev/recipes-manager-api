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
    public async Task CreateRecipeAsync([FromForm]RecipeCreateDto dto, CancellationToken cancellationToken)
    {
        await _recipesService.AddRecipeAsync(dto, cancellationToken);
    }

    [HttpPut]
    public async Task UpdateRecipeAsync([FromForm] RecipeDto dto, CancellationToken cancellationToken)
    {
        await _recipesService.UpdateRecipeAsync(dto, cancellationToken);
    }
}
