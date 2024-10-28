using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindNest.Migrations
{
    /// <inheritdoc />
    public partial class FixCreatedByForeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentPosts_AspNetUsers_CreatedUserId",
                table: "RentPosts");

            migrationBuilder.DropIndex(
                name: "IX_RentPosts_CreatedUserId",
                table: "RentPosts");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "RentPosts");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "RentPosts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentPosts_CreatedBy",
                table: "RentPosts",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_RentPosts_AspNetUsers_CreatedBy",
                table: "RentPosts",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentPosts_AspNetUsers_CreatedBy",
                table: "RentPosts");

            migrationBuilder.DropIndex(
                name: "IX_RentPosts_CreatedBy",
                table: "RentPosts");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "RentPosts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserId",
                table: "RentPosts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentPosts_CreatedUserId",
                table: "RentPosts",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentPosts_AspNetUsers_CreatedUserId",
                table: "RentPosts",
                column: "CreatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
