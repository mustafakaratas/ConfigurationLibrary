using ConfigurationApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationApi.Data;

public class ConfigurationDbContext : DbContext
{
    public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConfigurationDbContext).Assembly);
    }

    public DbSet<Configuration> Configurations { get; set; }
}