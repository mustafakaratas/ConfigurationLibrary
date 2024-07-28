using ConfigurationReader.BackgroundServices;
using ConfigurationReader.Data;
using ConfigurationReader.Services;
using ConfigurationReader.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationReader.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddConfigurationReader(this IServiceCollection serviceCollection,
        string applicationName,
        string connectionString,
        int refreshTimerIntervalInMs)
    {
        AppSettings.ApplicationName = applicationName;
        AppSettings.RefreshTimerIntervalInMs = refreshTimerIntervalInMs;

        serviceCollection
            .AddDbContext<ConfigurationDbContext>(x => x.UseSqlServer(connectionString))
            .AddHostedService<ConfigurationBackgroundService>()
            .AddScoped<IConfigurationService, ConfigurationService>()
            .AddMemoryCache();
    }
}