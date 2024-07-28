using ConfigurationReader.Constants;
using ConfigurationReader.Data;
using ConfigurationReader.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConfigurationReader.BackgroundServices;

public class ConfigurationBackgroundService : BackgroundService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ConfigurationBackgroundService> _logger;
    private readonly TimeSpan _period = TimeSpan.FromMilliseconds(AppSettings.RefreshTimerIntervalInMs);
    private readonly TimeSpan _cacheExpireTime = TimeSpan.FromHours(AppSettings.ConfigurationCacheHours);

    public ConfigurationBackgroundService(
        IMemoryCache memoryCache,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConfigurationBackgroundService> logger)
    {
        _memoryCache = memoryCache;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConfigurationBackgroundService initializing... Date: {0}", DateTime.UtcNow);

        await Refresh(stoppingToken);

        using var timer = new PeriodicTimer(_period);

        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            await Refresh(stoppingToken);
        }
    }

    private async Task Refresh(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConfigurationBackgroundService refresh started. Date: {0}", DateTime.UtcNow);

        using (var serviceScope = _serviceScopeFactory.CreateScope())
        {
            var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            var configurations = await configurationDbContext.Configurations
                .AsNoTracking()
                .Where(x => x.ApplicationName == AppSettings.ApplicationName &&
                            x.IsActive)
                .ToListAsync(cancellationToken: stoppingToken);

            _memoryCache.Set(CacheKeys.AllConfigurations, configurations, _cacheExpireTime);

            _logger.LogInformation("ConfigurationBackgroundService refresh finished. Date: {0}", DateTime.UtcNow);
        }
    }
}