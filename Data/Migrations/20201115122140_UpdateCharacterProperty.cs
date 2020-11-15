using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class UpdateCharacterProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_CharactersBaseStats_CBStatsID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CStatsId",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "CBStatsID",
                table: "Characters",
                newName: "CBStatsId");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_CBStatsID",
                table: "Characters",
                newName: "IX_Characters_CBStatsId");

            migrationBuilder.AlterColumn<int>(
                name: "CBStatsId",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_CharactersBaseStats_CBStatsId",
                table: "Characters",
                column: "CBStatsId",
                principalTable: "CharactersBaseStats",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_CharactersBaseStats_CBStatsId",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "CBStatsId",
                table: "Characters",
                newName: "CBStatsID");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_CBStatsId",
                table: "Characters",
                newName: "IX_Characters_CBStatsID");

            migrationBuilder.AlterColumn<int>(
                name: "CBStatsID",
                table: "Characters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CStatsId",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_CharactersBaseStats_CBStatsID",
                table: "Characters",
                column: "CBStatsID",
                principalTable: "CharactersBaseStats",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
