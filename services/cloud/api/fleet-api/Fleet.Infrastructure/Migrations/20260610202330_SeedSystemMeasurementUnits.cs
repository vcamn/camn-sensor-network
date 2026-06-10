using Fleet.Domain.Constants;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSystemMeasurementUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "metadata",
                table: "measurement_units",
                columns: ["id", "code", "unit_name", "description", "display_order"],
                values: new object[,]
                {
                    { MeasurementUnitIds.MicrogramsPerCubicMeter, "ug/m3", "Micrograms per cubic meter", "PM2.5 and black carbon", 1 },
                    { MeasurementUnitIds.MilesPerHour, "mph", "Miles per hour", "Wind speed", 2 },
                    { MeasurementUnitIds.Degrees, "Deg", "Degrees compass", "Wind direction", 3 },
                    { MeasurementUnitIds.PartsPerBillion, "ppb", "Parts per billion", "Total volatile organic compounds", 4 }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "metadata",
                table: "measurement_units",
                keyColumn: "id",
                keyValues:
                [
                    MeasurementUnitIds.MicrogramsPerCubicMeter,
                    MeasurementUnitIds.MilesPerHour,
                    MeasurementUnitIds.Degrees,
                    MeasurementUnitIds.PartsPerBillion
                ]
            );
        }
    }
}
