using Microsoft.EntityFrameworkCore.Migrations;

namespace MockPremierLeague.API.Migrations
{
    public partial class ModidiedFixturesModelWithKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HomeTeam",
                table: "Fixtures",
                newName: "HomeTeamId");

            migrationBuilder.RenameColumn(
                name: "AwayTeam",
                table: "Fixtures",
                newName: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_AwayTeamId",
                table: "Fixtures",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_HomeTeamId",
                table: "Fixtures",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_AwayTeamId",
                table: "Fixtures",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_HomeTeamId",
                table: "Fixtures",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_AwayTeamId",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_HomeTeamId",
                table: "Fixtures");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_AwayTeamId",
                table: "Fixtures");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_HomeTeamId",
                table: "Fixtures");

            migrationBuilder.RenameColumn(
                name: "HomeTeamId",
                table: "Fixtures",
                newName: "HomeTeam");

            migrationBuilder.RenameColumn(
                name: "AwayTeamId",
                table: "Fixtures",
                newName: "AwayTeam");
        }
    }
}
