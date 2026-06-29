using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMeasurementUnitsDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sensors_measurement_unit_measurement_unit_id",
                schema: "metadata",
                table: "sensors");

            migrationBuilder.AddForeignKey(
                name: "fk_sensors_measurement_units_measurement_unit_id",
                schema: "metadata",
                table: "sensors",
                column: "measurement_unit_id",
                principalSchema: "metadata",
                principalTable: "measurement_units",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sensors_measurement_units_measurement_unit_id",
                schema: "metadata",
                table: "sensors");

            migrationBuilder.AddForeignKey(
                name: "fk_sensors_measurement_unit_measurement_unit_id",
                schema: "metadata",
                table: "sensors",
                column: "measurement_unit_id",
                principalSchema: "metadata",
                principalTable: "measurement_units",
                principalColumn: "id");
        }
    }
}
