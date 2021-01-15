using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class UpdateMarket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Markets_ItemId",
                table: "Markets",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Markets_Items_ItemId",
                table: "Markets",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Markets_Items_ItemId",
                table: "Markets");

            migrationBuilder.DropIndex(
                name: "IX_Markets_ItemId",
                table: "Markets");
        }
    }
}
