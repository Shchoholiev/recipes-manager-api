using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RecipesManagerApi.Infrastructure.BackgroundServices;

public class HealthCheckBackgroundService : BackgroundService
{
    private readonly ILogger<HealthCheckBackgroundService> _logger;

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly IHostEnvironment _environment;

    public HealthCheckBackgroundService(
        ILogger<HealthCheckBackgroundService> logger, 
        IHttpClientFactory httpClientFactory,
        IHostEnvironment environment)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _environment = environment;
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
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
