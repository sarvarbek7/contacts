using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyActiveAssignedUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_phone_number_active_assigned_position_user_id",
                table: "phone_number",
                column: "active_assigned_position_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_phone_number_user_active_assigned_position_user_id",
                table: "phone_number",
                column: "active_assigned_position_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_phone_number_user_active_assigned_position_user_id",
                table: "phone_number");

            migrationBuilder.DropIndex(
                name: "ix_phone_number_active_assigned_position_user_id",
                table: "phone_number");
        }
    }
}
