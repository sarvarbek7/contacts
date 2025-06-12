using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_handbook_item",
                table: "handbook_item");

            migrationBuilder.DropColumn(
                name: "alias",
                table: "handbook_item");

            migrationBuilder.DropColumn(
                name: "name",
                table: "handbook");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "handbook_item",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_handbook_item",
                table: "handbook_item",
                column: "id");

            migrationBuilder.CreateTable(
                name: "handbook_item_translation",
                columns: table => new
                {
                    language = table.Column<int>(type: "integer", nullable: false),
                    handbook_item_id = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_handbook_item_translation", x => new { x.handbook_item_id, x.language });
                    table.ForeignKey(
                        name: "fk_handbook_item_translation_handbook_item_handbook_item_id",
                        column: x => x.handbook_item_id,
                        principalTable: "handbook_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "handbook_translation",
                columns: table => new
                {
                    language = table.Column<int>(type: "integer", nullable: false),
                    handbook_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_handbook_translation", x => new { x.handbook_id, x.language });
                    table.ForeignKey(
                        name: "fk_handbook_translation_handbook_handbook_id",
                        column: x => x.handbook_id,
                        principalTable: "handbook",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_handbook_item_handbook_id",
                table: "handbook_item",
                column: "handbook_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "handbook_item_translation");

            migrationBuilder.DropTable(
                name: "handbook_translation");

            migrationBuilder.DropPrimaryKey(
                name: "pk_handbook_item",
                table: "handbook_item");

            migrationBuilder.DropIndex(
                name: "ix_handbook_item_handbook_id",
                table: "handbook_item");

            migrationBuilder.DropColumn(
                name: "id",
                table: "handbook_item");

            migrationBuilder.AddColumn<string>(
                name: "alias",
                table: "handbook_item",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "handbook",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "pk_handbook_item",
                table: "handbook_item",
                columns: new[] { "handbook_id", "phone_number_id" });
        }
    }
}
