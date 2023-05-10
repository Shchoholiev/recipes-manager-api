using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.IO;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Identity;
using RecipesManagerApi.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using RecipesManagerApi.Application.Interfaces.Identity;
using RecipesManagerApi.Application.Models;

namespace RecipesManagerApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IUsersService _usersService;

    private readonly IRolesService _rolesService;

    private readonly ICloudStorageService _cloudStorageService;

    private readonly IRecipesService _recipesService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IUsersService usersService, IRolesService rolesService, ICloudStorageService cloudStorageService,
        IRecipesService recipesService)
    {
        _rolesService = rolesService;
        _usersService = usersService;
        _logger = logger;
        this._cloudStorageService = cloudStorageService;
        _recipesService = recipesService;
    }
   
    [HttpDelete("test-object-delete")]
    public async void TestCloudStorageDelete(string objectGuid, string fileExtension, CancellationToken cancellationToken)
    {
        Guid guid;
        Guid.TryParse(objectGuid, out guid);
        await this._cloudStorageService.DeleteFileAsync(guid, fileExtension, cancellationToken);
    }

    [HttpPost("test-object-upload")]
    public async void TestCloudStorageAdd(IFormFile file, CancellationToken cancellationToken)
    {
        Console.WriteLine(await this._cloudStorageService.UploadFileAsync(file, Guid.NewGuid(), file.FileName.Split(".").Last(), cancellationToken));
    }

    [Authorize]
    [HttpPost("recipe-add")]  
    public async Task CreateRecipeAsync([FromForm]RecipeCreateDto dto, CancellationToken cancellationToken)
    {
        await _recipesService.AddRecipeAsync(dto, cancellationToken);
    }

    [HttpPost("recipe-update")]
    public async Task UpdateRecipeAsync([FromForm] RecipeDto dto, CancellationToken cancellationToken)
    {
        await _recipesService.UpdateRecipeAsync(dto, cancellationToken);
    }
}