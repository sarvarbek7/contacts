using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePhoneNumberHnadbookItemRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_handbook_item_phone_number_phone_number_id",
                table: "handbook_item");

            migrationBuilder.DropIndex(
                name: "ix_handbook_item_phone_number_id",
                table: "handbook_item");

            migrationBuilder.DropColumn(
                name: "phone_number_id",
                table: "handbook_item");

            migrationBuilder.AddColumn<string>(
                name: "number",
                table: "handbook_item",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "number",
                table: "handbook_item");

            migrationBuilder.AddColumn<Guid>(
                name: "phone_number_id",
                table: "handbook_item",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_handbook_item_phone_number_id",
                table: "handbook_item",
                column: "phone_number_id");

            migrationBuilder.AddForeignKey(
                name: "fk_handbook_item_phone_number_phone_number_id",
                table: "handbook_item",
                column: "phone_number_id",
                principalTable: "phone_number",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
