using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindNest.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Addroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RentPostRooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    RentPostId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentPostRooms", x => new { x.RoomId, x.RentPostId });
                    table.ForeignKey(
                        name: "FK_RentPostRooms_RentPosts_RentPostId",
                        column: x => x.RentPostId,
                        principalTable: "RentPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentPostRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentPostRooms_RentPostId",
                table: "RentPostRooms",
                column: "RentPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentPostRooms");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
