using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindNest.Migrations
{
    /// <inheritdoc />
    public partial class AddorderfieldtoMediatable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Media");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Media",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Media");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Media",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Media",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Media",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Media",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Media",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
