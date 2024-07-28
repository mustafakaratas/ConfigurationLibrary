using ConfigurationApi.Data;
using ConfigurationApi.Data.Entities;
using ConfigurationApi.V1.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationApi.V1;

[ApiController]
[Route("api/v1/configurations")]
public class ConfigurationController : ControllerBase
{
    private readonly ConfigurationDbContext _configurationDbContext;
    private readonly ILogger<ConfigurationController> _logger;

    public ConfigurationController(
        ConfigurationDbContext configurationDbContext,
        ILogger<ConfigurationController> logger)
    {
        _configurationDbContext = configurationDbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> Query([FromQuery] string applicationName)
    {
        if (string.IsNullOrWhiteSpace(applicationName))
        {
            return BadRequest("Application name required");
        }

        var configurations = await _configurationDbContext.Configurations
            .AsNoTracking()
            .Where(x => x.ApplicationName == applicationName &&
                        x.IsActive)
            .ToListAsync();

        return Ok(configurations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than zero.");
        }

        var configuration = await _configurationDbContext.Configurations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (configuration is null)
        {
            return NotFound();
        }

        return Ok(configuration);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateConfigurationRequest request)
    {
        var configuration = new Configuration
        {
            Name = request.Name,
            Type = request.Type,
            Value = request.Value,
            IsActive = request.IsActive,
            ApplicationName = request.ApplicationName
        };

        _configurationDbContext.Configurations.Add(configuration);

        await _configurationDbContext.SaveChangesAsync();

        return Created();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateConfigurationRequest request)
    {
        var configuration = await _configurationDbContext.Configurations.FirstOrDefaultAsync(x => x.Id == id);

        if (configuration is null)
        {
            return NotFound();
        }

        configuration.Name = request.Name;
        configuration.Type = request.Type;
        configuration.Value = request.Value;
        configuration.IsActive = request.IsActive;
        configuration.ApplicationName = request.ApplicationName;

        try
        {
            await _configurationDbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException exp)
        {
            _logger.LogError("Concurrency exception occurred when updating the configuration. ExceptionMessage: {0}", exp.Message);

            return Conflict("Please try again");
        }

        return NoContent();
    }
}