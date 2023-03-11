using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

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

    private readonly IRolesService _rolesService;

    private readonly IUsersService _usersService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IRolesService rolesService, IUsersService usersService)
    {
        _rolesService = rolesService;
        _logger = logger;
        _usersService = usersService;
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

    [HttpGet("test")]
    public async void Test(CancellationToken cancellationToken)
    {
        var res =  await this._usersService.GetPageUsersAsync(new PageParameters() { PageNumber = 1, PageSize = 3}, cancellationToken);
    }
}
