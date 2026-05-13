using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mhwildsdb.Migrations
{
    /// <inheritdoc />
    public partial class AddedArmourSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArmourSetId",
                schema: "app",
                table: "Armours",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArmourSets",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SetBonusSkillId = table.Column<Guid>(type: "uuid", nullable: true),
                    GroupBonusSkillId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmourSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmourSets_Skills_GroupBonusSkillId",
                        column: x => x.GroupBonusSkillId,
                        principalSchema: "app",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ArmourSets_Skills_SetBonusSkillId",
                        column: x => x.SetBonusSkillId,
                        principalSchema: "app",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Armours_ArmourSetId",
                schema: "app",
                table: "Armours",
                column: "ArmourSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmourSets_GroupBonusSkillId",
                schema: "app",
                table: "ArmourSets",
                column: "GroupBonusSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmourSets_Name",
                schema: "app",
                table: "ArmourSets",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArmourSets_SetBonusSkillId",
                schema: "app",
                table: "ArmourSets",
                column: "SetBonusSkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Armours_ArmourSets_ArmourSetId",
                schema: "app",
                table: "Armours",
                column: "ArmourSetId",
                principalSchema: "app",
                principalTable: "ArmourSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Armours_ArmourSets_ArmourSetId",
                schema: "app",
                table: "Armours");

            migrationBuilder.DropTable(
                name: "ArmourSets",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_Armours_ArmourSetId",
                schema: "app",
                table: "Armours");

            migrationBuilder.DropColumn(
                name: "ArmourSetId",
                schema: "app",
                table: "Armours");
        }
    }
}
