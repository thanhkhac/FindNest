using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindNest.Migrations
{
    /// <inheritdoc />
    public partial class Addindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RentPosts_CreatedAt",
                table: "RentPosts",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentPosts_CreatedAt",
                table: "RentPosts");
        }
    }
}
