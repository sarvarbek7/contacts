using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionUserHistoryRemoveLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                table: "position_user_phone_number",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "removed_by_id",
                table: "position_user_phone_number",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_position_user_phone_number_removed_by_id",
                table: "position_user_phone_number",
                column: "removed_by_id");

            migrationBuilder.AddForeignKey(
                name: "fk_position_user_phone_number_account_removed_by_id",
                table: "position_user_phone_number",
                column: "removed_by_id",
                principalTable: "account",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_position_user_phone_number_account_removed_by_id",
                table: "position_user_phone_number");

            migrationBuilder.DropIndex(
                name: "ix_position_user_phone_number_removed_by_id",
                table: "position_user_phone_number");

            migrationBuilder.DropColumn(
                name: "removed_at",
                table: "position_user_phone_number");

            migrationBuilder.DropColumn(
                name: "removed_by_id",
                table: "position_user_phone_number");
        }
    }
}
