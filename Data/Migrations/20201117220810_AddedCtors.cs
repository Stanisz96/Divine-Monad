using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class AddedCtors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllAtributes",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "Critic",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "HP",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "IsConsumable",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Armor",
                table: "CharactersBaseStats");

            migrationBuilder.DropColumn(
                name: "Attack",
                table: "CharactersBaseStats");

            migrationBuilder.DropColumn(
                name: "Critic",
                table: "CharactersBaseStats");

            migrationBuilder.DropColumn(
                name: "HP",
                table: "CharactersBaseStats");

            migrationBuilder.AlterColumn<int>(
                name: "Strength",
                table: "ItemsStats",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Stamina",
                table: "ItemsStats",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AttackMin",
                table: "ItemsStats",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AttackMax",
                table: "ItemsStats",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Armor",
                table: "ItemsStats",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Agility",
                table: "ItemsStats",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Accuracy",
                table: "ItemsStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Block",
                table: "ItemsStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CritChance",
                table: "ItemsStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dexterity",
                table: "ItemsStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dodge",
                table: "ItemsStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HitPoints",
                table: "ItemsStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Luck",
                table: "ItemsStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "ItemsStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RarityId",
                table: "ItemCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LootedHeroic",
                table: "CharactersGameStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LootedLegendary",
                table: "CharactersGameStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LootedNormal",
                table: "CharactersGameStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LootedUnique",
                table: "CharactersGameStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dexterity",
                table: "CharactersBaseStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Luck",
                table: "CharactersBaseStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Characters",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Rarity",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Chance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rarity", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_RarityId",
                table: "ItemCategories",
                column: "RarityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCategories_Rarity_RarityId",
                table: "ItemCategories",
                column: "RarityId",
                principalTable: "Rarity",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCategories_Rarity_RarityId",
                table: "ItemCategories");

            migrationBuilder.DropTable(
                name: "Rarity");

            migrationBuilder.DropIndex(
                name: "IX_ItemCategories_RarityId",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Accuracy",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "CritChance",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "Dexterity",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "Dodge",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "HitPoints",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "Luck",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "ItemsStats");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RarityId",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "LootedHeroic",
                table: "CharactersGameStats");

            migrationBuilder.DropColumn(
                name: "LootedLegendary",
                table: "CharactersGameStats");

            migrationBuilder.DropColumn(
                name: "LootedNormal",
                table: "CharactersGameStats");

            migrationBuilder.DropColumn(
                name: "LootedUnique",
                table: "CharactersGameStats");

            migrationBuilder.DropColumn(
                name: "Dexterity",
                table: "CharactersBaseStats");

            migrationBuilder.DropColumn(
                name: "Luck",
                table: "CharactersBaseStats");

            migrationBuilder.AlterColumn<int>(
                name: "Strength",
                table: "ItemsStats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Stamina",
                table: "ItemsStats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AttackMin",
                table: "ItemsStats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AttackMax",
                table: "ItemsStats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Armor",
                table: "ItemsStats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Agility",
                table: "ItemsStats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "AllAtributes",
                table: "ItemsStats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Critic",
                table: "ItemsStats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HP",
                table: "ItemsStats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConsumable",
                table: "ItemCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Armor",
                table: "CharactersBaseStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Attack",
                table: "CharactersBaseStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Critic",
                table: "CharactersBaseStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HP",
                table: "CharactersBaseStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }
    }
}
