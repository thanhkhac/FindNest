using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FindNest.Migrations
{
    /// <inheritdoc />
    public partial class AdddefaultdataRentCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RentCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Phòng trọ" },
                    { 2, "Căn hộ" },
                    { 3, "Nguyên Căn" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RentCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RentCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RentCategories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
