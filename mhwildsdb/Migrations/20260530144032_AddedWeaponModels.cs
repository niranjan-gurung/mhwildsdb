using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mhwildsdb.Migrations
{
    /// <inheritdoc />
    public partial class AddedWeaponModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weapons",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    WeaponType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Defense = table.Column<int>(type: "integer", nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    Slots = table.Column<int[]>(type: "integer[]", nullable: false),
                    Affinity = table.Column<int>(type: "integer", nullable: false),
                    RawDamage = table.Column<int>(type: "integer", nullable: false),
                    DisplayDamage = table.Column<int>(type: "integer", nullable: false),
                    SharpnessRed = table.Column<int>(type: "integer", nullable: true),
                    SharpnessOrange = table.Column<int>(type: "integer", nullable: true),
                    SharpnessYellow = table.Column<int>(type: "integer", nullable: true),
                    SharpnessGreen = table.Column<int>(type: "integer", nullable: true),
                    SharpnessBlue = table.Column<int>(type: "integer", nullable: true),
                    SharpnessWhite = table.Column<int>(type: "integer", nullable: true),
                    SharpnessPurple = table.Column<int>(type: "integer", nullable: true),
                    ShellType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ShellPower = table.Column<int>(type: "integer", nullable: true),
                    KinsectLevel = table.Column<int>(type: "integer", nullable: true),
                    PhialType = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    PhialRawDamage = table.Column<int>(type: "integer", nullable: true),
                    PhialDisplayDamage = table.Column<int>(type: "integer", nullable: true),
                    Coatings = table.Column<int[]>(type: "integer[]", nullable: true),
                    SpecialAmmo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponAmmo",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Rapid = table.Column<bool>(type: "boolean", nullable: true),
                    WeaponId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponAmmo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponAmmo_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalSchema: "app",
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponSkillRanks",
                schema: "app",
                columns: table => new
                {
                    SkillRanksId = table.Column<Guid>(type: "uuid", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponSkillRanks", x => new { x.SkillRanksId, x.WeaponId });
                    table.ForeignKey(
                        name: "FK_WeaponSkillRanks_SkillRanks_SkillRanksId",
                        column: x => x.SkillRanksId,
                        principalSchema: "app",
                        principalTable: "SkillRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeaponSkillRanks_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalSchema: "app",
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponSpecials",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Element = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Status = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    RawDamage = table.Column<int>(type: "integer", nullable: false),
                    DisplayDamage = table.Column<int>(type: "integer", nullable: false),
                    Hidden = table.Column<bool>(type: "boolean", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponSpecials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponSpecials_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalSchema: "app",
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeaponAmmo_WeaponId",
                schema: "app",
                table: "WeaponAmmo",
                column: "WeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_Name",
                schema: "app",
                table: "Weapons",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSkillRanks_WeaponId",
                schema: "app",
                table: "WeaponSkillRanks",
                column: "WeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSpecials_WeaponId",
                schema: "app",
                table: "WeaponSpecials",
                column: "WeaponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeaponAmmo",
                schema: "app");

            migrationBuilder.DropTable(
                name: "WeaponSkillRanks",
                schema: "app");

            migrationBuilder.DropTable(
                name: "WeaponSpecials",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Weapons",
                schema: "app");
        }
    }
}
