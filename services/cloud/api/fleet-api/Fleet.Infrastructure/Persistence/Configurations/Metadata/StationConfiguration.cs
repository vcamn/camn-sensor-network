using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class StationConfiguration : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        builder.ToTable("stations");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.StationCode)
               .IsRequired()
               .HasMaxLength(128);

        builder.Property(e => e.Description)
               .IsRequired(false)
               .HasMaxLength(2048);

        builder.HasOne(e => e.Site)
               .WithMany(s => s.Stations)
               .HasForeignKey(e => e.SiteId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
