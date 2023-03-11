using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanCity.Migrations
{
    /// <inheritdoc />
    public partial class addIsCleaned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCleaned",
                table: "PointOnTheMaps",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCleaned",
                table: "PointOnTheMaps");
        }
    }
}
