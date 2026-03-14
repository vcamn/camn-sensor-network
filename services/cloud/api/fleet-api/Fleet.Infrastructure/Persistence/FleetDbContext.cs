using System;
using Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Persistence;

public class FleetDbContext : DbContext
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options)
        : base(options)
    {
    }

    public DbSet<Site> Sites => Set<Site>();
    public DbSet<SiteStatus> SiteStatuses => Set<SiteStatus>();
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<StationStatus> StationStatuses => Set<StationStatus>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceStatus> DeviceStatuses => Set<DeviceStatus>();
    public DbSet<IntegrationTemplate> IntegrationTemplates => Set<IntegrationTemplate>();
    public DbSet<IntegrationType> IntegrationTypes => Set<IntegrationType>();
    public DbSet<SensorType> SensorTypes => Set<SensorType>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<SensorStatus> SensorStatuses => Set<SensorStatus>();
    public DbSet<SensorIntegration> SensorIntegrations => Set<SensorIntegration>();
    public DbSet<SensorCalibration> SensorCalibrations => Set<SensorCalibration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("metadata");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FleetDbContext).Assembly);
    }
}
