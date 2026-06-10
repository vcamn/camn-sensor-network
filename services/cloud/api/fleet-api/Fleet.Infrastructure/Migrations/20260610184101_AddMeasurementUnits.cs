using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMeasurementUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit_of_measure",
                schema: "metadata",
                table: "sensors");

            migrationBuilder.AddColumn<Guid>(
                name: "measurement_unit_id",
                schema: "metadata",
                table: "sensors",
                type: "uuid",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "measurement_units",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "citext", maxLength: 16, nullable: false),
                    unit_name = table.Column<string>(type: "citext", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_measurement_units", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sensors_measurement_unit_id",
                schema: "metadata",
                table: "sensors",
                column: "measurement_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_measurement_units_code",
                schema: "metadata",
                table: "measurement_units",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_measurement_units_unit_name",
                schema: "metadata",
                table: "measurement_units",
                column: "unit_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_sensors_measurement_unit_measurement_unit_id",
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
                name: "fk_sensors_measurement_unit_measurement_unit_id",
                schema: "metadata",
                table: "sensors");

            migrationBuilder.DropTable(
                name: "measurement_units",
                schema: "metadata");

            migrationBuilder.DropIndex(
                name: "ix_sensors_measurement_unit_id",
                schema: "metadata",
                table: "sensors");

            migrationBuilder.DropColumn(
                name: "measurement_unit_id",
                schema: "metadata",
                table: "sensors");

            migrationBuilder.AddColumn<string>(
                name: "unit_of_measure",
                schema: "metadata",
                table: "sensors",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }
    }
}
