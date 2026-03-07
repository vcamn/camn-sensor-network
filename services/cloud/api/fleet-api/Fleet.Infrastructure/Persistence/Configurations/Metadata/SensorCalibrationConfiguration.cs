using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class SensorCalibrationConfiguration : IEntityTypeConfiguration<SensorCalibration>
{
    public void Configure(EntityTypeBuilder<SensorCalibration> builder)
    {
        builder.ToTable("sensor_calibrations");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CalibrationMethod)
            .IsRequired(false)
            .HasMaxLength(128);

        builder.Property(e => e.PerformedBy)
            .IsRequired(false)
            .HasMaxLength(128);

        builder.HasOne(e => e.Sensor)
            .WithMany(s => s.SensorCalibrations)
            .HasForeignKey(e => e.SensorId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
