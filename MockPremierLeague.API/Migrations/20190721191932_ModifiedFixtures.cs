using Microsoft.EntityFrameworkCore.Migrations;

namespace MockPremierLeague.API.Migrations
{
    public partial class ModifiedFixtures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FixtureURL",
                table: "Fixtures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixtureURL",
                table: "Fixtures");
        }
    }
}
