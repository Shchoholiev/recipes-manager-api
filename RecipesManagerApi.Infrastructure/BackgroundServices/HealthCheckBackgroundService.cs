using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Domain.Enums;

namespace RecipesManagerApi.Infrastructure.BackgroundServices;

public class HealthCheckBackgroundService : BackgroundService
{
    private readonly ILogger<HealthCheckBackgroundService> _logger;

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly IHostEnvironment _environment;

    private readonly ILogsService _logsService;

    public HealthCheckBackgroundService(
        ILogger<HealthCheckBackgroundService> logger, 
        IHttpClientFactory httpClientFactory,
        IHostEnvironment environment,
        ILogsService logsService)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _environment = environment;
        _logsService = logsService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                // find a better way to determine url
                var url = _environment.IsDevelopment() 
                    ? "https://sh-recipes-manager-api-dev.azurewebsites.net/health" 
                    : "https://sh-recipes-manager-api.azurewebsites.net/health";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation($"Health check succeeded. {url}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed.");
                _logsService.AddLogAsync(new LogDto { Text = ex.Message, Level = LogLevels.Critical }, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
