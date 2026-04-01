using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedConfigurableStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SITE STATUS
            migrationBuilder.InsertData(
                schema: "metadata",
                table: "site_statuses",
                columns: new[] { "id", "code", "status_name", "description", "display_order" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "PROPOSED", "Proposed", "Site is proposed", 1 },
                    { Guid.NewGuid(), "APPROVED", "Approved", "Site approved for deployment", 2 },
                    { Guid.NewGuid(), "ACTIVE", "Active", "Site operational", 3 },
                    { Guid.NewGuid(), "SERVICING", "Servicing", "Site under maintenance", 4 },
                    { Guid.NewGuid(), "RETIRED", "Retired", "Site retired", 5 }
                });

            // STATION STATUS
            migrationBuilder.InsertData(
                schema: "metadata",
                table: "station_statuses",
                columns: new[] { "id", "code", "status_name", "description", "display_order" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "ACTIVE", "Active", "Station operational", 1 },
                    { Guid.NewGuid(), "INACTIVE", "Inactive", "Station inactive", 2 },
                    { Guid.NewGuid(), "MAINTENANCE", "Maintenance", "Under maintenance", 3 },
                    { Guid.NewGuid(), "OFFLINE", "Offline", "Not reporting", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM metadata.site_statuses WHERE code IN ('PROPOSED', 'APPROVED', 'ACTIVE', 'SERVICING', 'RETIRED');");
            migrationBuilder.Sql("DELETE FROM metadata.station_statuses WHERE code IN ('ACTIVE', 'INACTIVE', 'MAINTENANCE', 'OFFLINE');");
        }
    }
}
