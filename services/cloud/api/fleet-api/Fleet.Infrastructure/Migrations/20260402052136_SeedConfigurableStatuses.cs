using Fleet.Infrastructure.Persistence.SeedData;
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
                columns: ["id", "code", "status_name", "description", "display_order"],
                values: new object[,]
                {
                    { Guid.NewGuid(), "PROPOSED", "Proposed", "Site is proposed", 1 },
                    { Guid.NewGuid(), "APPROVED", "Approved", "Site approved for deployment", 2 },
                    { Guid.NewGuid(), "ACTIVE", "Active", "Site operational", 3 },
                    { Guid.NewGuid(), "SERVICING", "Servicing", "Site under maintenance", 4 },
                    { Guid.NewGuid(), "RETIRED", "Retired", "Site retired", 5 }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "metadata",
                table: "site_statuses",
                keyColumn: "id",
                keyValues: StationStatusSeed.Data.Select(s => (object)s.Id).ToArray()
            );
        }
    }
}
