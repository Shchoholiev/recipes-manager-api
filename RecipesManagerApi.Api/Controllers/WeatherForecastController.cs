using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IUsersService usersService, IRolesService rolesService, IUserManager userManager)
    {
        _rolesService = rolesService;
        _usersService = usersService;
        _logger = logger;
        _userManager = userManager;
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
}
