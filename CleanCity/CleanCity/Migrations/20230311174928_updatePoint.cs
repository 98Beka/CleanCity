using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanCity.Migrations
{
    /// <inheritdoc />
    public partial class updatePoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequestCleaned",
                table: "PointOnTheMaps",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequestDelete",
                table: "PointOnTheMaps",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequestCleaned",
                table: "PointOnTheMaps");

            migrationBuilder.DropColumn(
                name: "IsRequestDelete",
                table: "PointOnTheMaps");
        }
    }
}
