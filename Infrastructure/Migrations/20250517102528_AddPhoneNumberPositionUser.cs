using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneNumberPositionUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "active_assigned_position_user_id",
                table: "phone_number",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "position_user_phone_number",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    phone_number_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_position_user_phone_number", x => x.id);
                    table.ForeignKey(
                        name: "fk_position_user_phone_number_account_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_position_user_phone_number_phone_number_phone_number_id",
                        column: x => x.phone_number_id,
                        principalTable: "phone_number",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_position_user_phone_number_created_by_id",
                table: "position_user_phone_number",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_position_user_phone_number_phone_number_id",
                table: "position_user_phone_number",
                column: "phone_number_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "position_user_phone_number");

            migrationBuilder.DropColumn(
                name: "active_assigned_position_user_id",
                table: "phone_number");
        }
    }
}
