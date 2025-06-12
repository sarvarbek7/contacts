using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHandbooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "handbook",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_handbook", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "handbook_item",
                columns: table => new
                {
                    phone_number_id = table.Column<Guid>(type: "uuid", nullable: false),
                    handbook_id = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_handbook_item", x => new { x.handbook_id, x.phone_number_id });
                    table.ForeignKey(
                        name: "fk_handbook_item_handbook_handbook_id",
                        column: x => x.handbook_id,
                        principalTable: "handbook",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_handbook_item_phone_number_phone_number_id",
                        column: x => x.phone_number_id,
                        principalTable: "phone_number",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_handbook_item_phone_number_id",
                table: "handbook_item",
                column: "phone_number_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "handbook_item");

            migrationBuilder.DropTable(
                name: "handbook");
        }
    }
}
