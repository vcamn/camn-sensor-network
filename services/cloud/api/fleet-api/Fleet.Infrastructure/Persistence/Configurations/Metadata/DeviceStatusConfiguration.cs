using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class DeviceStatusConfiguration : IEntityTypeConfiguration<DeviceStatus>
{
    public void Configure(EntityTypeBuilder<DeviceStatus> builder)
    {
        builder.ToTable("device_statuses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.StatusName)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(e => e.Description)
            .HasMaxLength(64);
    }
}
