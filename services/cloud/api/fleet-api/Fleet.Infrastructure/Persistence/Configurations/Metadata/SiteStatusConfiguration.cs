using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class SiteStatusConfiguration : IEntityTypeConfiguration<SiteStatus>
{
    public void Configure(EntityTypeBuilder<SiteStatus> builder)
    {
        builder.ToTable("site_statuses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.StatusName)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(e => e.Description)
            .HasMaxLength(256);
    }
}
