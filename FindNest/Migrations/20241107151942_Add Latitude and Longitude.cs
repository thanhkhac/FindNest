using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindNest.Migrations
{
    /// <inheritdoc />
    public partial class AddLatitudeandLongitude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Latitude",
                table: "RentPosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Longitude",
                table: "RentPosts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "RentPosts");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "RentPosts");
        }
    }
}
