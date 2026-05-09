using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mhwildsdb.Migrations
{
    /// <inheritdoc />
    public partial class RenamedArmourSkillRanksColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArmourSkillRanks_SkillRanks_SkillsId",
                schema: "app",
                table: "ArmourSkillRanks");

            migrationBuilder.RenameColumn(
                name: "SkillsId",
                schema: "app",
                table: "ArmourSkillRanks",
                newName: "SkillRanksId");

            migrationBuilder.RenameIndex(
                name: "IX_ArmourSkillRanks_SkillsId",
                schema: "app",
                table: "ArmourSkillRanks",
                newName: "IX_ArmourSkillRanks_SkillRanksId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArmourSkillRanks_SkillRanks_SkillRanksId",
                schema: "app",
                table: "ArmourSkillRanks",
                column: "SkillRanksId",
                principalSchema: "app",
                principalTable: "SkillRanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArmourSkillRanks_SkillRanks_SkillRanksId",
                schema: "app",
                table: "ArmourSkillRanks");

            migrationBuilder.RenameColumn(
                name: "SkillRanksId",
                schema: "app",
                table: "ArmourSkillRanks",
                newName: "SkillsId");

            migrationBuilder.RenameIndex(
                name: "IX_ArmourSkillRanks_SkillRanksId",
                schema: "app",
                table: "ArmourSkillRanks",
                newName: "IX_ArmourSkillRanks_SkillsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArmourSkillRanks_SkillRanks_SkillsId",
                schema: "app",
                table: "ArmourSkillRanks",
                column: "SkillsId",
                principalSchema: "app",
                principalTable: "SkillRanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
