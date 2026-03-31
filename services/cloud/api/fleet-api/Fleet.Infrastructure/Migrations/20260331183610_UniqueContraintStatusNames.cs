using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueContraintStatusNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_station_statuses_status_name",
                schema: "metadata",
                table: "station_statuses",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_site_statuses_status_name",
                schema: "metadata",
                table: "site_statuses",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sensor_statuses_status_name",
                schema: "metadata",
                table: "sensor_statuses",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_device_statuses_status_name",
                schema: "metadata",
                table: "device_statuses",
                column: "status_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_station_statuses_status_name",
                schema: "metadata",
                table: "station_statuses");

            migrationBuilder.DropIndex(
                name: "ix_site_statuses_status_name",
                schema: "metadata",
                table: "site_statuses");

            migrationBuilder.DropIndex(
                name: "ix_sensor_statuses_status_name",
                schema: "metadata",
                table: "sensor_statuses");

            migrationBuilder.DropIndex(
                name: "ix_device_statuses_status_name",
                schema: "metadata",
                table: "device_statuses");
        }
    }
}
