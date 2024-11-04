using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindNest.Migrations
{
    /// <inheritdoc />
    public partial class Addindexrentpost2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RentPosts_Area",
                table: "RentPosts",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_RentPosts_IsNegotiatedPrice_Price",
                table: "RentPosts",
                columns: new[] { "IsNegotiatedPrice", "Price" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentPosts_Area",
                table: "RentPosts");

            migrationBuilder.DropIndex(
                name: "IX_RentPosts_IsNegotiatedPrice_Price",
                table: "RentPosts");
        }
    }
}
