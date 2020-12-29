using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class StatsPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatsPoints",
                table: "CharactersBaseStats",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatsPoints",
                table: "CharactersBaseStats");
        }
    }
}
