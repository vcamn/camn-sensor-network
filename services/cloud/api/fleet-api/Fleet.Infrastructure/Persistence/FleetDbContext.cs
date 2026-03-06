using System;
using Fleet.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Persistence;

public class FleetDbContext : DbContext
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options)
        : base(options)
    {
    }

    public DbSet<Site> Sites => Set<Site>();
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<SensorCalibration> SensorCalibrations => Set<SensorCalibration>();
}
