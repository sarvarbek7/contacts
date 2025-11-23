using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PhoneNumberPositionManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_phone_number_user_active_assigned_position_user_id",
                table: "phone_number");

            migrationBuilder.DropForeignKey(
                name: "fk_phone_number_user_active_assigned_user_id",
                table: "phone_number");

            migrationBuilder.DropTable(
                name: "position_phone_number");

            migrationBuilder.DropTable(
                name: "position_user_phone_number");

            migrationBuilder.DropTable(
                name: "user_phone_number");

            migrationBuilder.DropIndex(
                name: "ix_phone_number_active_assigned_position_user_id",
                table: "phone_number");

            migrationBuilder.DropIndex(
                name: "ix_phone_number_active_assigned_user_id",
                table: "phone_number");

            migrationBuilder.DropColumn(
                name: "active_assigned_organization_id",
                table: "phone_number");

            migrationBuilder.DropColumn(
                name: "active_assigned_position_id",
                table: "phone_number");

            migrationBuilder.DropColumn(
                name: "active_assigned_position_user_id",
                table: "phone_number");

            migrationBuilder.DropColumn(
                name: "active_assigned_user_id",
                table: "phone_number");

            migrationBuilder.CreateTable(
                name: "user_assignment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    phone_number_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_assignment", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_assignment_account_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_user_assignment_phone_number_phone_number_id",
                        column: x => x.phone_number_id,
                        principalTable: "phone_number",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_assignment_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "position_assignment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    position_id = table.Column<int>(type: "integer", nullable: false),
                    organization_id = table.Column<int>(type: "integer", nullable: false),
                    department_id = table.Column<int>(type: "integer", nullable: false),
                    inner_position_id = table.Column<int>(type: "integer", nullable: false),
                    phone_number_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: true),
                    user_assignment_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_position_assignment", x => x.id);
                    table.ForeignKey(
                        name: "fk_position_assignment_account_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_position_assignment_phone_number_phone_number_id",
                        column: x => x.phone_number_id,
                        principalTable: "phone_number",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_position_assignment_user_assignment_user_assignment_id",
                        column: x => x.user_assignment_id,
                        principalTable: "user_assignment",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "position_assignment_user",
                columns: table => new
                {
                    position_assignments_id = table.Column<Guid>(type: "uuid", nullable: false),
                    users_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_position_assignment_user", x => new { x.position_assignments_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_position_assignment_user_position_assignment_position_assig",
                        column: x => x.position_assignments_id,
                        principalTable: "position_assignment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_position_assignment_user_user_users_id",
                        column: x => x.users_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_phone_number_number",
                table: "phone_number",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_position_assignment_created_by_id",
                table: "position_assignment",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_position_assignment_phone_number_id",
                table: "position_assignment",
                column: "phone_number_id");

            migrationBuilder.CreateIndex(
                name: "ix_position_assignment_user_assignment_id",
                table: "position_assignment",
                column: "user_assignment_id");

            migrationBuilder.CreateIndex(
                name: "ix_position_assignment_user_users_id",
                table: "position_assignment_user",
                column: "users_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_assignment_created_by_id",
                table: "user_assignment",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_assignment_phone_number_id",
                table: "user_assignment",
                column: "phone_number_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_assignment_user_id",
                table: "user_assignment",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "position_assignment_user");

            migrationBuilder.DropTable(
                name: "position_assignment");

            migrationBuilder.DropTable(
                name: "user_assignment");

            migrationBuilder.DropIndex(
                name: "ix_phone_number_number",
                table: "phone_number");

            migrationBuilder.AddColumn<int>(
                name: "active_assigned_organization_id",
                table: "phone_number",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "active_assigned_position_id",
                table: "phone_number",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "active_assigned_position_user_id",
                table: "phone_number",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "active_assigned_user_id",
                table: "phone_number",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "position_phone_number",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by_id = table.Column<int>(type: "integer", nullable: true),
                    phone_number_id = table.Column<Guid>(type: "uuid", nullable: false),
                    removed_by_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    department = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    organization = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<string>(type: "text", nullable: false),
                    position_id = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "position_user_phone_number",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by_id = table.Column<int>(type: "integer", nullable: true),
                    phone_number_id = table.Column<Guid>(type: "uuid", nullable: false),
                    removed_by_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    removed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
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
                        name: "fk_position_user_phone_number_account_removed_by_id",
                        column: x => x.removed_by_id,
                        principalTable: "account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_position_user_phone_number_phone_number_phone_number_id",
                        column: x => x.phone_number_id,
                        principalTable: "phone_number",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_phone_number",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by_id = table.Column<int>(type: "integer", nullable: true),
                    phone_number_id = table.Column<Guid>(type: "uuid", nullable: false),
                    removed_by_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    removed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_phone_number", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_phone_number_account_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_user_phone_number_account_removed_by_id",
                        column: x => x.removed_by_id,
                        principalTable: "account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_phone_number_phone_number_phone_number_id",
                        column: x => x.phone_number_id,
                        principalTable: "phone_number",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_phone_number_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_phone_number_active_assigned_position_user_id",
                table: "phone_number",
                column: "active_assigned_position_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_number_active_assigned_user_id",
                table: "phone_number",
                column: "active_assigned_user_id");

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

            migrationBuilder.CreateIndex(
                name: "ix_position_user_phone_number_created_by_id",
                table: "position_user_phone_number",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_position_user_phone_number_phone_number_id",
                table: "position_user_phone_number",
                column: "phone_number_id");

            migrationBuilder.CreateIndex(
                name: "ix_position_user_phone_number_removed_by_id",
                table: "position_user_phone_number",
                column: "removed_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_phone_number_created_by_id",
                table: "user_phone_number",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_phone_number_phone_number_id",
                table: "user_phone_number",
                column: "phone_number_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_phone_number_removed_by_id",
                table: "user_phone_number",
                column: "removed_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_phone_number_user_id",
                table: "user_phone_number",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_phone_number_user_active_assigned_position_user_id",
                table: "phone_number",
                column: "active_assigned_position_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_phone_number_user_active_assigned_user_id",
                table: "phone_number",
                column: "active_assigned_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
