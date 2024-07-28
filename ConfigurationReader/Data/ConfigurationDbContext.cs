using ConfigurationReader.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationReader.Data;

public class ConfigurationDbContext : DbContext
{
    public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    public DbSet<Configuration> Configurations { get; set; }
}