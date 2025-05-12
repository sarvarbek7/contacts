using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeRoleRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "active_assigned_position_id",
                table: "phone_number",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "account",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "position_phone_number",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    phone_number_id = table.Column<Guid>(type: "uuid", nullable: false),
                    position_id = table.Column<int>(type: "integer", nullable: false),
                    organization = table.Column<string>(type: "text", nullable: false),
                    department = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: true),
                    removed_by_id = table.Column<int>(type: "integer", nullable: true),
                    removed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_position_phone_number", x => x.id);
                    table.ForeignKey(
                        name: "fk_position_phone_number_account_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_position_phone_number_account_removed_by_id",
                        column: x => x.removed_by_id,
                        principalTable: "account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_position_phone_number_phone_number_phone_number_id",
                        column: x => x.phone_number_id,
                        principalTable: "phone_number",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_position_phone_number_created_by_id",
                table: "position_phone_number",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_position_phone_number_phone_number_id",
                table: "position_phone_number",
                column: "phone_number_id");

            migrationBuilder.CreateIndex(
                name: "ix_position_phone_number_removed_by_id",
                table: "position_phone_number",
                column: "removed_by_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "position_phone_number");

            migrationBuilder.DropColumn(
                name: "active_assigned_position_id",
                table: "phone_number");

            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "account",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
