using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mhwildsdb.Migrations
{
    /// <inheritdoc />
    public partial class AddedDecorationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Decorations",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    Slot = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decorations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DecorationSkillRanks",
                schema: "app",
                columns: table => new
                {
                    DecorationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillRanksId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecorationSkillRanks", x => new { x.DecorationId, x.SkillRanksId });
                    table.ForeignKey(
                        name: "FK_DecorationSkillRanks_Decorations_DecorationId",
                        column: x => x.DecorationId,
                        principalSchema: "app",
                        principalTable: "Decorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DecorationSkillRanks_SkillRanks_SkillRanksId",
                        column: x => x.SkillRanksId,
                        principalSchema: "app",
                        principalTable: "SkillRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Decorations_Name",
                schema: "app",
                table: "Decorations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DecorationSkillRanks_SkillRanksId",
                schema: "app",
                table: "DecorationSkillRanks",
                column: "SkillRanksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecorationSkillRanks",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Decorations",
                schema: "app");
        }
    }
}
