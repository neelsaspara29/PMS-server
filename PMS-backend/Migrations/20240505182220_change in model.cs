using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_backend.Migrations
{
    /// <inheritdoc />
    public partial class changeinmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "active_status",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "active_status",
                table: "PatientReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active_status",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "active_status",
                table: "PatientReports");
        }
    }
}
