namespace ConfigurationReader.Services;

public interface IConfigurationService
{
    Task<object> GetValue<T>(string key);
}