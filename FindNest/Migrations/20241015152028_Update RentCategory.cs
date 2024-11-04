using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindNest.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRentCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentPosts_RentCategory_RentCategoryId",
                table: "RentPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentCategory",
                table: "RentCategory");

            migrationBuilder.RenameTable(
                name: "RentCategory",
                newName: "RentCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentCategories",
                table: "RentCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentPosts_RentCategories_RentCategoryId",
                table: "RentPosts",
                column: "RentCategoryId",
                principalTable: "RentCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentPosts_RentCategories_RentCategoryId",
                table: "RentPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentCategories",
                table: "RentCategories");

            migrationBuilder.RenameTable(
                name: "RentCategories",
                newName: "RentCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentCategory",
                table: "RentCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentPosts_RentCategory_RentCategoryId",
                table: "RentPosts",
                column: "RentCategoryId",
                principalTable: "RentCategory",
                principalColumn: "Id");
        }
    }
}
