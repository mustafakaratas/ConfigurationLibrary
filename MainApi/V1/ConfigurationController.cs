using ConfigurationReader.Services;
using Microsoft.AspNetCore.Mvc;
using ValueType = MainApi.Enums.ValueType;

namespace MainApi.V1;

[ApiController]
[Route("api/v1/configurations")]
public class ConfigurationController : ControllerBase
{
    private readonly IConfigurationService _configurationService;

    public ConfigurationController(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    [HttpGet("{key}")]
    public async Task<ActionResult> Get([FromRoute] string key, [FromQuery] ValueType valueType)
    {
        var value = await GetValueByType(valueType, key);

        return Ok(new
        {
            value
        });
    }

    private async Task<object> GetValueByType(ValueType valueType, string key)
    {
        return valueType switch
        {
            ValueType.String => await _configurationService.GetValue<string>(key),
            ValueType.Integer => await _configurationService.GetValue<int>(key),
            ValueType.Boolean => await _configurationService.GetValue<bool>(key),
            _ => throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null)
        };
    }
}