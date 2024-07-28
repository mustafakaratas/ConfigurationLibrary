using System.ComponentModel;
using ConfigurationReader.Constants;
using ConfigurationReader.Data;
using ConfigurationReader.Data.Entities;
using ConfigurationReader.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ConfigurationReader.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ConfigurationDbContext _configurationDbContext;
    private readonly ILogger<ConfigurationService> _logger;

    public ConfigurationService(
        IMemoryCache memoryCache,
        ConfigurationDbContext configurationDbContext,
        ILogger<ConfigurationService> logger)
    {
        _memoryCache = memoryCache;
        _configurationDbContext = configurationDbContext;
        _logger = logger;
    }

    public async Task<object> GetValue<T>(string key)
    {
        Configuration configuration;

        if (_memoryCache.TryGetValue<List<Configuration>>(CacheKeys.AllConfigurations, out var configurations))
        {
            configuration = configurations!.FirstOrDefault(x => x.Name == key);
        }
        else
        {
            configuration = await _configurationDbContext.Configurations
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.ApplicationName == AppSettings.ApplicationName &&
                    x.IsActive &&
                    x.Name == key);
        }

        if (configuration is null)
        {
            return null;
        }

        var result = ConvertValue<T>(configuration.Value);

        return result;
    }

    private object ConvertValue<T>(string value)
    {
        try
        {
            var typeConverter = TypeDescriptor.GetConverter(typeof(T));
            return (T)typeConverter.ConvertFromInvariantString(value)!;
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred when converting configuration value. Value: {0}, ExceptionMessage: {1}", value, e.Message);
            return null;
        }
    }
}