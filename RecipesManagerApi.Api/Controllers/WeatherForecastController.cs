using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RecipesManagerApi.Application.IServices;
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

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IUsersService usersService, IRolesService rolesService)
    {
        _rolesService = rolesService;
        _usersService = usersService;
        _logger = logger;
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

    [HttpGet("test1")]
    public async void TestUserAdding(CancellationToken cancellationToken)
    {
        var role = await this._rolesService.GetRoleAsync(new ObjectId("640cfe0bb72023aa1124c0ca"), cancellationToken);
        await this._usersService.AddUserAsync(new UserDto() { Name = "larry", Phone = "5465456321", Email = " asdfsdf@gmail.com", RefreshToken = "yes", RefreshTokenExpiryDate = DateTime.Now, AppleDeviceId = new Guid(), WebId = new Guid(), Roles = new List<RoleDto>() { role} }, cancellationToken);
    }
}
