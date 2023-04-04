using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.IO;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.Access;
using RecipesManagerApi.Application.Models.Identity;
using RecipesManagerApi.Application.Models.Login;
using RecipesManagerApi.Application.Models.Register;

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

    private readonly IUserManager _userManager;

    private readonly ICloudStorageService _cloudStorageService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IUsersService usersService, IRolesService rolesService, IUserManager userManager, ICloudStorageService cloudStorageService)
    {
        _rolesService = rolesService;
        _usersService = usersService;
        _logger = logger;
        _userManager = userManager;
        _cloudStorageService = cloudStorageService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("test-guest-add")]
    public async Task<TokensModel> TestGuestAdding(CancellationToken cancellationToken)
    {
        var guest = new AccessAppleGuestModel
        {
            Name = "testmobile",
            AppleDeviceId = Guid.NewGuid(),
        };

        return await this._userManager.AccessAppleGuestAsync(guest, cancellationToken);
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
}
