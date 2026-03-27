using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class SensorTypeConfiguration : IEntityTypeConfiguration<SensorType>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<SensorType> builder)
    {
        builder.ToTable("sensor_types");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.TypeName)
               .IsRequired()
               .HasMaxLength(32);

        builder.Property(e => e.Description)
               .HasMaxLength(256);

        builder.Property(e => e.CreatedAtUtc)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
