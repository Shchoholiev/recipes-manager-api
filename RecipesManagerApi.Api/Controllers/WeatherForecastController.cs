using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.IO;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;

namespace RecipesManagerApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly Guid guid;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IUsersService _usersService;

    private readonly IRolesService _rolesService;

    private readonly ICloudStorageService _cloudStorageService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IUsersService usersService, IRolesService rolesService, ICloudStorageService cloudStorageService)
    {
        _rolesService = rolesService;
        _usersService = usersService;
        _logger = logger;
        this._cloudStorageService = cloudStorageService;
        guid = new Guid();
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

    [HttpGet("test-user-add")]
    public async void TestUserAdding(CancellationToken cancellationToken)
    {
        var role = await this._rolesService.GetRoleAsync(new ObjectId("640cfe0bb72023aa1124c0ca"), cancellationToken);
        await this._usersService.AddUserAsync(new UserDto() { Name = "larry", Phone = "5465456321", Email = " asdfsdf@gmail.com", RefreshToken = "yes", RefreshTokenExpiryDate = DateTime.Now, AppleDeviceId = new Guid(), WebId = Guid.NewGuid(), Roles = new List<RoleDto>() { role} }, cancellationToken);
    }

    [HttpGet("test-object-delete")]
    public async void TestCloudStorageDelete(CancellationToken cancellationToken)
    {
        await this._cloudStorageService.DeleteFileAsync(guid, cancellationToken);
    }

    [HttpGet("test-object-upload")]
    public async void TestCloudStorageAdd(CancellationToken cancellationToken)
    {
        var filePath = @"/Users/vitaliy/Desktop/image.jpg";
        using var stream = new MemoryStream(System.IO.File.ReadAllBytes(filePath).ToArray());
        var formFile = new FormFile(stream, 0, stream.Length, (guid.ToString() + Path.GetExtension(filePath)), (guid.ToString() + Path.GetExtension(filePath)));        
        Console.WriteLine(await this._cloudStorageService.UploadFileAsync(formFile, cancellationToken));
    }
}
