using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePositionHistoryProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "department",
                table: "position_phone_number");

            migrationBuilder.DropColumn(
                name: "organization",
                table: "position_phone_number");

            migrationBuilder.DropColumn(
                name: "position",
                table: "position_phone_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "department",
                table: "position_phone_number",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "organization",
                table: "position_phone_number",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "position",
                table: "position_phone_number",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
