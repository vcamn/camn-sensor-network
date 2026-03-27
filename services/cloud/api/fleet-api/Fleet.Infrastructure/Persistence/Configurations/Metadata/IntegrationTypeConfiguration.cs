using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class IntegrationTypeConfiguration : IEntityTypeConfiguration<IntegrationType>
{
    public void Configure(EntityTypeBuilder<IntegrationType> builder)
    {
        builder.ToTable("integration_types");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.TypeName)
               .IsRequired()
               .HasMaxLength(32);

        builder.Property(e => e.Description)
               .HasMaxLength(256);
    }
}
