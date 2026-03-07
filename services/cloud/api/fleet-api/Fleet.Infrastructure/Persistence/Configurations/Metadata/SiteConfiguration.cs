using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("sites");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.SiteName)
               .IsRequired()
               .HasMaxLength(128);

        builder.Property(e => e.City)
               .IsRequired()
               .HasMaxLength(128);

        builder.Property(e => e.State)
               .IsRequired()
               .HasMaxLength(64);

        builder.Property(e => e.AddressLine1)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(e => e.PostalCode)
               .IsRequired(false)
               .HasMaxLength(16);

        builder.Property(e => e.DeploymentStatus)
               .IsRequired()
               .HasMaxLength(32);
    }
}
