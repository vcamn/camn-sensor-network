using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteCodeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "site_code",
                schema: "metadata",
                table: "sites",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "site_code",
                schema: "metadata",
                table: "sites");
        }
    }
}
