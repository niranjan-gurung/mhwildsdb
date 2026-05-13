using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mhwildsdb.Migrations
{
    /// <inheritdoc />
    public partial class AddedSkillRankNamesAndSetPieceRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "app",
                table: "SkillRanks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetPieceRequired",
                schema: "app",
                table: "SkillRanks",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "app",
                table: "SkillRanks");

            migrationBuilder.DropColumn(
                name: "SetPieceRequired",
                schema: "app",
                table: "SkillRanks");
        }
    }
}
