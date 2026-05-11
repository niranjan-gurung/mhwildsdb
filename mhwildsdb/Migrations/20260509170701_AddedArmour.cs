using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mhwildsdb.Migrations
{
    /// <inheritdoc />
    public partial class AddedArmour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Armours",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Piece = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Rank = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    Defense = table.Column<int>(type: "integer", nullable: false),
                    FireResistance = table.Column<int>(type: "integer", nullable: false),
                    WaterResistance = table.Column<int>(type: "integer", nullable: false),
                    IceResistance = table.Column<int>(type: "integer", nullable: false),
                    ThunderResistance = table.Column<int>(type: "integer", nullable: false),
                    DragonResistance = table.Column<int>(type: "integer", nullable: false),
                    Slots = table.Column<int[]>(type: "integer[]", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArmourSkillRanks",
                schema: "app",
                columns: table => new
                {
                    ArmoursId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmourSkillRanks", x => new { x.ArmoursId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_ArmourSkillRanks_Armours_ArmoursId",
                        column: x => x.ArmoursId,
                        principalSchema: "app",
                        principalTable: "Armours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmourSkillRanks_SkillRanks_SkillsId",
                        column: x => x.SkillsId,
                        principalSchema: "app",
                        principalTable: "SkillRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Armours_Name",
                schema: "app",
                table: "Armours",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArmourSkillRanks_SkillsId",
                schema: "app",
                table: "ArmourSkillRanks",
                column: "SkillsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmourSkillRanks",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Armours",
                schema: "app");
        }
    }
}
