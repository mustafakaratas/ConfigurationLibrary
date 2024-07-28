using ConfigurationApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConfigurationApi.Data.Mappings;

public class ConfigurationMapping : IEntityTypeConfiguration<Configuration>
{
    public void Configure(EntityTypeBuilder<Configuration> builder)
    {
        builder.ToTable("Configurations");
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Name)
            .HasColumnType("nvarchar(255)")
            .IsRequired();

        builder.Property(p => p.Type)
            .HasColumnType("nvarchar(255)")
            .IsRequired();

        builder.Property(p => p.Value)
            .HasColumnType("nvarchar(255)")
            .IsRequired();

        builder.Property(p => p.IsActive)
            .HasColumnType("bit")
            .IsRequired();

        builder.Property(p => p.ApplicationName)
            .HasColumnType("nvarchar(255)")
            .IsRequired();

        builder.Property(p => p.Timest)
            .HasColumnType("byte[]")
            .IsRowVersion();
    }
}