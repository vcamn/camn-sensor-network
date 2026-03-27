using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Infrastructure.Persistence.Configurations.Metadata;

public class IntegrationTemplateConfiguration : IEntityTypeConfiguration<IntegrationTemplate>
{
    public void Configure(EntityTypeBuilder<IntegrationTemplate> builder)
    {
        builder.ToTable("integration_templates");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Description)
            .HasMaxLength(256);

        builder.Property(e => e.DefaultConfigJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(e => e.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(e => e.IntegrationType)
            .WithMany(it => it.IntegrationTemplates)
            .HasForeignKey(e => e.IntegrationTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.SensorType)
            .WithMany(st => st.IntegrationTemplates)
            .HasForeignKey(e => e.SensorTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
