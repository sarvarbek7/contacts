using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_account_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_account_account_deleted_by_id",
                        column: x => x.deleted_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_account_account_updated_by_id",
                        column: x => x.updated_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    external_id = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    last_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    photo = table.Column<string>(type: "text", nullable: true),
                    account_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "phone_number",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    active_assigned_user_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by_id = table.Column<int>(type: "integer", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_phone_number", x => x.id);
                    table.ForeignKey(
                        name: "fk_phone_number_account_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_phone_number_account_deleted_by_id",
                        column: x => x.deleted_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_phone_number_account_updated_by_id",
                        column: x => x.updated_by_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_phone_number_user_active_assigned_user_id",
                        column: x => x.active_assigned_user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "user_phone_number",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    phone_number_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: true)
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
                name: "ix_account_created_by_id",
                table: "account",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_deleted_by_id",
                table: "account",
                column: "deleted_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_updated_by_id",
                table: "account",
                column: "updated_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_number_active_assigned_user_id",
                table: "phone_number",
                column: "active_assigned_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_number_created_by_id",
                table: "phone_number",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_number_deleted_by_id",
                table: "phone_number",
                column: "deleted_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_number_updated_by_id",
                table: "phone_number",
                column: "updated_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_account_id",
                table: "user",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_external_id",
                table: "user",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_phone_number_created_by_id",
                table: "user_phone_number",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_phone_number_phone_number_id",
                table: "user_phone_number",
                column: "phone_number_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_phone_number_user_id",
                table: "user_phone_number",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_phone_number");

            migrationBuilder.DropTable(
                name: "phone_number");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "account");
        }
    }
}
