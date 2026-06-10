using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class MeasurementUnitConfiguration : IEntityTypeConfiguration<MeasurementUnit>
{
    public void Configure(EntityTypeBuilder<MeasurementUnit> builder)
    {
        builder.ToTable("measurement_units");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
               .IsRequired()
               .HasMaxLength(16)
               .HasColumnType("citext");

        builder.Property(e => e.UnitName)
               .IsRequired()
               .HasMaxLength(64)
               .HasColumnType("citext");

        builder.Property(e => e.Description)
               .HasMaxLength(256);

        builder.Property(e => e.DisplayOrder)
               .IsRequired();

        builder.HasIndex(e => e.Code)
               .IsUnique();

        builder.HasIndex(e => e.UnitName)
               .IsUnique();
    }
}