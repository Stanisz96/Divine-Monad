using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class UpdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "ItemCategories");

            migrationBuilder.AddColumn<int>(
                name: "StatisticsId",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Armor",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Arrow",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Boots",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Bow",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Gloves",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Helmet",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConsumable",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Shield",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Weapon1H",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Weapon2H",
                table: "ItemCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemCategories",
                table: "ItemCategories",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "CharactersBaseStats",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(nullable: false),
                    Experience = table.Column<int>(nullable: false),
                    HP = table.Column<int>(nullable: false),
                    Attack = table.Column<int>(nullable: false),
                    Armor = table.Column<int>(nullable: false),
                    Critic = table.Column<int>(nullable: false),
                    Strength = table.Column<int>(nullable: false),
                    Stamina = table.Column<int>(nullable: false),
                    Agility = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharactersBaseStats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CharactersGameStats",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonsterKills = table.Column<int>(nullable: false),
                    CollectedGold = table.Column<int>(nullable: false),
                    DeathsNumber = table.Column<int>(nullable: false),
                    LostFights = table.Column<int>(nullable: false),
                    WinFights = table.Column<int>(nullable: false),
                    DrawFights = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharactersGameStats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CharactersItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    IsEquipped = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharactersItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ItemsStats",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HP = table.Column<int>(nullable: true),
                    AttackMin = table.Column<int>(nullable: true),
                    AttackMax = table.Column<int>(nullable: true),
                    Armor = table.Column<int>(nullable: true),
                    Critic = table.Column<int>(nullable: true),
                    Strength = table.Column<int>(nullable: true),
                    Stamina = table.Column<int>(nullable: true),
                    Agility = table.Column<int>(nullable: true),
                    AllAtributes = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsStats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    CStatsId = table.Column<int>(nullable: false),
                    CBStatsID = table.Column<int>(nullable: true),
                    GStatsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Characters_CharactersBaseStats_CBStatsID",
                        column: x => x.CBStatsID,
                        principalTable: "CharactersBaseStats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_CharactersGameStats_GStatsId",
                        column: x => x.GStatsId,
                        principalTable: "CharactersGameStats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_StatisticsId",
                table: "Items",
                column: "StatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CBStatsID",
                table: "Characters",
                column: "CBStatsID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_GStatsId",
                table: "Characters",
                column: "GStatsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "ItemCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemsStats_StatisticsId",
                table: "Items",
                column: "StatisticsId",
                principalTable: "ItemsStats",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemsStats_StatisticsId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "CharactersItems");

            migrationBuilder.DropTable(
                name: "ItemsStats");

            migrationBuilder.DropTable(
                name: "CharactersBaseStats");

            migrationBuilder.DropTable(
                name: "CharactersGameStats");

            migrationBuilder.DropIndex(
                name: "IX_Items_StatisticsId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemCategories",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "StatisticsId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Armor",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Arrow",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Boots",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Bow",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Gloves",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Helmet",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "IsConsumable",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Shield",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Weapon1H",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "Weapon2H",
                table: "ItemCategories");

            migrationBuilder.RenameTable(
                name: "ItemCategories",
                newName: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
