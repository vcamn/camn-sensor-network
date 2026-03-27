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

       builder.Property(e => e.AddressLine1)
              .IsRequired()
              .HasMaxLength(256);

       builder.Property(e => e.AddressLine2)
              .IsRequired(false)
              .HasMaxLength(256);

       builder.Property(e => e.City)
              .IsRequired()
              .HasMaxLength(128);

       builder.Property(e => e.State)
              .IsRequired()
              .HasMaxLength(8)
              .HasDefaultValue("CA");
              
       builder.Property(e => e.PostalCode)
              .IsRequired()
              .HasMaxLength(16);

       builder.Property(e => e.Country)
              .IsRequired()
              .HasMaxLength(24)
              .HasDefaultValue("USA");

       builder.Property(e => e.ContactName)
              .IsRequired()
              .HasMaxLength(128);

       builder.Property(e => e.ContactEmail)
              .IsRequired()
              .HasMaxLength(256);

       builder.Property(e => e.CreatedAtUtc)
              .HasDefaultValueSql("CURRENT_TIMESTAMP");

       builder.HasOne(e => e.Status)
              .WithMany(s => s.Sites)
              .HasForeignKey(e => e.SiteStatusId)
              .OnDelete(DeleteBehavior.NoAction);
    }
}
