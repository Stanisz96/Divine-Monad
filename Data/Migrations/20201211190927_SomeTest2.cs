using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class SomeTest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Characters_Name",
                table: "Characters",
                column: "Name",
                unique: true);
        }
    }
}
