using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class SensorStatusConfiguration : IEntityTypeConfiguration<SensorStatus>
{
    public void Configure(EntityTypeBuilder<SensorStatus> builder)
    {
        builder.ToTable("sensor_statuses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasColumnType("citext")
            .HasMaxLength(16);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.Property(e => e.StatusName)
            .IsRequired()
            .HasColumnType("citext")
            .HasMaxLength(32);

        builder.HasIndex(e => e.StatusName)
            .IsUnique();

        builder.Property(e => e.Description)
            .HasMaxLength(64);
    }
}
