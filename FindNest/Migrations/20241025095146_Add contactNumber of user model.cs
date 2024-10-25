using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindNest.Migrations
{
    /// <inheritdoc />
    public partial class AddcontactNumberofusermodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactPhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPhoneNumber",
                table: "AspNetUsers");
        }
    }
}
