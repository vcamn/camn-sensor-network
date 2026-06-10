using Fleet.Domain.Constants;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSystemSensorTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "metadata",
                table: "sensor_types",
                columns: ["id", "type_name", "description", "display_order", "is_active"],
                values: new object[,]
                {
                    { SensorTypeIds.AethLabs, "AethLabs", "Measures PM2.5 and black carbon concentrations in ambient air.", 1, true },
                    { SensorTypeIds.AggieAir, "AggieAir", "Measures total volatile organic compounds (TVOCs) in ambient air.", 2, true },
                    { SensorTypeIds.LTWind, "LTWind", "Measures wind speed and wind direction.", 3, true },
                    { SensorTypeIds.PurpleAir, "PurpleAir", "Measures PM2.5 particulate matter concentrations for air quality monitoring.", 4, true },
                    { SensorTypeIds.Other, "Other", "Generic sensor type for unsupported or custom sensors.", 5, true }
                }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "metadata",
                table: "sensor_types",
                keyColumn: "id",
                keyValues:
                [
                    SensorTypeIds.AethLabs,
                    SensorTypeIds.AggieAir,
                    SensorTypeIds.LTWind,
                    SensorTypeIds.PurpleAir,
                    SensorTypeIds.Other
                ]
            );
        }
    }
}
