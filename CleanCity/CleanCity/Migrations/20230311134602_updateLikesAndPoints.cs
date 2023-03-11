using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanCity.Migrations
{
    /// <inheritdoc />
    public partial class updateLikesAndPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "PointOnTheMaps",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Likes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "PointOnTheMaps");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Likes");
        }
    }
}
