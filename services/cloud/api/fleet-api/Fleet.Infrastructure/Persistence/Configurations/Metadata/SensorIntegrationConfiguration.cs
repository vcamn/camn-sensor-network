using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class SensorIntegrationConfiguration : IEntityTypeConfiguration<SensorIntegration>
{
    public void Configure(EntityTypeBuilder<SensorIntegration> builder)
    {
       builder.ToTable("sensor_integrations");

       builder.HasKey(e => e.Id);

       builder.Property(e => e.ConfigJson)
              .IsRequired()
              .HasColumnType("jsonb");

       builder.Property(e => e.CreatedAtUtc)
              .HasDefaultValueSql("CURRENT_TIMESTAMP");

       builder.HasOne(e => e.Sensor)
              .WithMany(s => s.SensorIntegrations)
              .HasForeignKey(e => e.SensorId)
              .OnDelete(DeleteBehavior.NoAction);

       builder.HasOne(e => e.IntegrationType)
              .WithMany(t => t.SensorIntegrations)
              .HasForeignKey(e => e.IntegrationTypeId)
              .OnDelete(DeleteBehavior.NoAction);

       builder.HasOne(e => e.IntegrationTemplate)
              .WithMany(t => t.SensorIntegrations)
              .HasForeignKey(e => e.IntegrationTemplateId)
              .OnDelete(DeleteBehavior.NoAction);
    }
}
