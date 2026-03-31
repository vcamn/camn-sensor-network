using Fleet.Domain.Constants;
using Fleet.Domain.Entities;
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
            
            // DEVICE STATUS
            migrationBuilder.InsertData(
                schema: "metadata",
                table: "device_statuses",
                columns: new[] { "id", "code", "status_name", "description", "display_order" },
                values: new object[,]
                {
                    { DeviceStatusIds.Active, "ACTIVE", "Active", "Device is operating normally", 1 },
                    { DeviceStatusIds.Inactive, "INACTIVE", "Inactive", "Device is not active", 2 },
                    { DeviceStatusIds.Offline, "OFFLINE", "Offline", "Device is offline", 3 },
                    { DeviceStatusIds.Error, "ERROR", "Error", "Device is in error state", 4 },
                    { DeviceStatusIds.Provisioning, "PROVISIONING", "Provisioning", "Device is being provisioned", 5 },
                    { DeviceStatusIds.Maintenance, "MAINTENANCE", "Maintenance", "Device under maintenance", 6 },
                    { DeviceStatusIds.Retired, "RETIRED", "Retired", "Device retired", 7 }
                });

            migrationBuilder.InsertData(
                schema: "metadata",
                table: "sensor_statuses",
                columns: new[] { "id", "code", "status_name", "description", "display_order" },
                values: new object[,]
                {
                    { SensorStatusIds.Active, "ACTIVE", "Active", "Sensor is operating normally", 1 },
                    { SensorStatusIds.Inactive, "INACTIVE", "Inactive", "Sensor is not active", 2 },
                    { SensorStatusIds.Offline, "OFFLINE", "Offline", "Sensor is offline", 3 },
                    { SensorStatusIds.Error, "ERROR", "Error", "Sensor is in error state", 4 },
                    { SensorStatusIds.Provisioning, "PROVISIONING", "Provisioning", "Sensor is being provisioned", 5 },
                    { SensorStatusIds.Maintenance, "MAINTENANCE", "Maintenance", "Sensor under maintenance", 6 },
                    { SensorStatusIds.Retired, "RETIRED", "Retired", "Sensor retired", 7 }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "metadata",
                table: "device_statuses",
                keyColumn: "id",
                keyValues: new object[]
                {
                    DeviceStatusIds.Active,
                    DeviceStatusIds.Inactive,
                    DeviceStatusIds.Offline,
                    DeviceStatusIds.Error,
                    DeviceStatusIds.Provisioning,
                    DeviceStatusIds.Maintenance,
                    DeviceStatusIds.Retired
                });

            migrationBuilder.DeleteData(
                schema: "metadata",
                table: "sensor_statuses",
                keyColumn: "id",
                keyValues: new object[]                {
                    SensorStatusIds.Active,
                    SensorStatusIds.Inactive,
                    SensorStatusIds.Offline,
                    SensorStatusIds.Error,
                    SensorStatusIds.Provisioning,
                    SensorStatusIds.Maintenance,
                    SensorStatusIds.Retired
                });
        }
    }
}
