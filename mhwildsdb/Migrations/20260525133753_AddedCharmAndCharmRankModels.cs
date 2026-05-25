using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mhwildsdb.Migrations
{
    /// <inheritdoc />
    public partial class AddedCharmAndCharmRankModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Charms",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharmRanks",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    CharmId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharmRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharmRanks_Charms_CharmId",
                        column: x => x.CharmId,
                        principalSchema: "app",
                        principalTable: "Charms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharmRankSkillRanks",
                schema: "app",
                columns: table => new
                {
                    CharmRankId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillRankId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharmRankSkillRanks", x => new { x.CharmRankId, x.SkillRankId });
                    table.ForeignKey(
                        name: "FK_CharmRankSkillRanks_CharmRanks_CharmRankId",
                        column: x => x.CharmRankId,
                        principalSchema: "app",
                        principalTable: "CharmRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharmRankSkillRanks_SkillRanks_SkillRankId",
                        column: x => x.SkillRankId,
                        principalSchema: "app",
                        principalTable: "SkillRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharmRanks_CharmId",
                schema: "app",
                table: "CharmRanks",
                column: "CharmId");

            migrationBuilder.CreateIndex(
                name: "IX_CharmRankSkillRanks_SkillRankId",
                schema: "app",
                table: "CharmRankSkillRanks",
                column: "SkillRankId");

            migrationBuilder.CreateIndex(
                name: "IX_Charms_Name",
                schema: "app",
                table: "Charms",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharmRankSkillRanks",
                schema: "app");

            migrationBuilder.DropTable(
                name: "CharmRanks",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Charms",
                schema: "app");
        }
    }
}
