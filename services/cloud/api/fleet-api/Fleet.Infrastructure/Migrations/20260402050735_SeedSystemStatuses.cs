using Fleet.Domain.Constants;
using Fleet.Infrastructure.Persistence.SeedData;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSystemStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "metadata",
                table: "station_statuses",
                columns: ["id", "code", "status_name", "description", "display_order"],
                values: new object[,] 
                {
                    { StationStatusIds.Active, "ACTIVE", "Active", "Station operational", 1 },
                    { StationStatusIds.Inactive, "INACTIVE", "Inactive", "Station should be operating but is not (problem)", 2 },
                    { StationStatusIds.Offline, "OFFLINE", "Offline", "Station is operating offline", 3 },
                    { StationStatusIds.Maintenance, "MAINTENANCE", "Maintenance", "Under maintenance", 4 },
                    { StationStatusIds.Decommissioned, "DECOMMISSIONED", "Decommissioned", "Station permanently retired", 5 }
                }
            );

            // DEVICE STATUS
            migrationBuilder.InsertData(
                schema: "metadata",
                table: "device_statuses",
                columns: ["id", "code", "status_name", "description", "display_order"],
                values: new object[,]
                {
                    { DeviceStatusIds.Active, "ACTIVE", "Active", "Device is operating normally", 1 },
                    { DeviceStatusIds.Inactive, "INACTIVE", "Inactive", "Device should be operating but is not (problem)", 2 },
                    { DeviceStatusIds.Disabled, "DISABLED", "Disabled", "Device intentionally turned off", 3 },
                    { DeviceStatusIds.Offline, "OFFLINE", "Offline", "Device is operating offline", 4 },
                    { DeviceStatusIds.Error, "ERROR", "Error", "Device is in fault state", 5 },
                    { DeviceStatusIds.Provisioning, "PROVISIONING", "Provisioning", "Device is being provisioned (set up and configured)", 6 },
                    { DeviceStatusIds.Maintenance, "MAINTENANCE", "Maintenance", "Device under maintenance", 7 },
                    { DeviceStatusIds.Decommissioned, "DECOMMISSIONED", "Decommissioned", "Device permanently retired", 8 }

                }
            );

            // SENSOR STATUS
            migrationBuilder.InsertData(
                schema: "metadata",
                table: "sensor_statuses",
                columns: ["id", "code", "status_name", "description", "display_order"],
                values: new object[,]
                {
                    { SensorStatusIds.Active, "ACTIVE", "Active", "Sensor is operating normally", 1 },
                    { SensorStatusIds.Inactive, "INACTIVE", "Inactive", "Sensor should be operating but is not (problem)", 2 },
                    { SensorStatusIds.Disabled, "DISABLED", "Disabled", "Sensor intentionally turned off", 3 },
                    { SensorStatusIds.Offline, "OFFLINE", "Offline", "Sensor is operating offline", 4 },
                    { SensorStatusIds.Error, "ERROR", "Error", "Sensor is in fault state", 5 },
                    { SensorStatusIds.Provisioning, "PROVISIONING", "Provisioning", "Sensor is being provisioned (set up and configured)", 6 },
                    { SensorStatusIds.Maintenance, "MAINTENANCE", "Maintenance", "Sensor under maintenance", 7 },
                    { SensorStatusIds.Decommissioned, "DECOMMISSIONED", "Decommissioned", "Sensor permanently retired", 8 }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.DeleteData(
                    schema: "metadata",
                    table: "station_statuses",
                    keyColumn: "id",
                    keyValues: StationStatusSeed.Data.Select(s => (object)s.Id).ToArray()
                );

            migrationBuilder.DeleteData(
                schema: "metadata",
                table: "device_statuses",
                keyColumn: "id",
                keyValues: DeviceStatusSeed.Data.Select(s => (object)s.Id).ToArray()
            );

            migrationBuilder.DeleteData(
                schema: "metadata",
                table: "sensor_statuses",
                keyColumn: "id",
                keyValues: SensorStatusSeed.Data.Select(s => (object)s.Id).ToArray()
            );
        }
    }
}
