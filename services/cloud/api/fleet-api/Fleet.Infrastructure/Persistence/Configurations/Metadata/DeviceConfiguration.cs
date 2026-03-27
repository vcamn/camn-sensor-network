using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
       builder.ToTable("devices");

       builder.HasKey(e => e.Id);

       builder.Property(e => e.DeviceIdentifier)
              .IsRequired()
              .HasMaxLength(48);

       builder.Property(e => e.DeviceType)
              .IsRequired()
              .HasMaxLength(32);

       builder.Property(e => e.SerialNumber)
              .IsRequired(false)
              .HasMaxLength(64);

       builder.Property(e => e.MacAddress)
              .IsRequired(false)
              .HasMaxLength(24);

       builder.Property(e => e.IoTHubDeviceId)
              .IsRequired(false)
              .HasMaxLength(128)
              .HasColumnName("iot_hub_device_id");

       builder.Property(e => e.FirmwareVersion)
              .IsRequired(false)
              .HasMaxLength(32);

       builder.Property(e => e.CreatedAtUtc)
              .HasDefaultValueSql("CURRENT_TIMESTAMP");

       builder.HasOne(e => e.DeviceStatus)
              .WithMany(ds => ds.Devices)
              .HasForeignKey(e => e.DeviceStatusId)
              .IsRequired()
              .OnDelete(DeleteBehavior.NoAction);

       builder.HasOne(e => e.Station)
              .WithMany(s => s.Devices)
              .HasForeignKey(e => e.StationId)
              .IsRequired()
              .OnDelete(DeleteBehavior.NoAction);
    }
}
