using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class BpSlots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BpSlotId",
                table: "CharactersItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BpSlots",
                table: "CharactersBaseStats",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BpSlotId",
                table: "CharactersItems");

            migrationBuilder.DropColumn(
                name: "BpSlots",
                table: "CharactersBaseStats");
        }
    }
}
