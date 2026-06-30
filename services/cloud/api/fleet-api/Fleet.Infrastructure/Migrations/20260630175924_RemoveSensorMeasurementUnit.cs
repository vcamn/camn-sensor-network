using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSensorMeasurementUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sensors_measurement_units_measurement_unit_id",
                schema: "metadata",
                table: "sensors");

            migrationBuilder.DropIndex(
                name: "ix_sensors_measurement_unit_id",
                schema: "metadata",
                table: "sensors");

            migrationBuilder.DropColumn(
                name: "measurement_unit_id",
                schema: "metadata",
                table: "sensors");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at_utc",
                schema: "metadata",
                table: "sensors",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                schema: "metadata",
                table: "sensors",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at_utc",
                schema: "metadata",
                table: "sensors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                schema: "metadata",
                table: "sensors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<Guid>(
                name: "measurement_unit_id",
                schema: "metadata",
                table: "sensors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_sensors_measurement_unit_id",
                schema: "metadata",
                table: "sensors",
                column: "measurement_unit_id");

            migrationBuilder.AddForeignKey(
                name: "fk_sensors_measurement_units_measurement_unit_id",
                schema: "metadata",
                table: "sensors",
                column: "measurement_unit_id",
                principalSchema: "metadata",
                principalTable: "measurement_units",
                principalColumn: "id");
        }
    }
}
