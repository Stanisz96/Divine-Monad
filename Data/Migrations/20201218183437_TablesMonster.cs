using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class TablesMonster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gold",
                table: "CharactersBaseStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MonstersLoot",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonsterId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonstersLoot", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MonstersStats",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stamina = table.Column<int>(nullable: false),
                    Strength = table.Column<int>(nullable: false),
                    Agility = table.Column<int>(nullable: false),
                    Dexterity = table.Column<int>(nullable: false),
                    Luck = table.Column<int>(nullable: false),
                    HitPoints = table.Column<int>(nullable: false),
                    AttackMin = table.Column<int>(nullable: false),
                    Attack = table.Column<int>(nullable: false),
                    AttackMax = table.Column<int>(nullable: false),
                    Armor = table.Column<int>(nullable: false),
                    Block = table.Column<int>(nullable: false),
                    Dodge = table.Column<int>(nullable: false),
                    Speed = table.Column<int>(nullable: false),
                    CritChance = table.Column<int>(nullable: false),
                    Accuracy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonstersStats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Monsters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Gold = table.Column<int>(nullable: false),
                    Experience = table.Column<int>(nullable: false),
                    MonsterStatsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monsters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Monsters_MonstersStats_MonsterStatsId",
                        column: x => x.MonsterStatsId,
                        principalTable: "MonstersStats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Monsters_MonsterStatsId",
                table: "Monsters",
                column: "MonsterStatsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Monsters");

            migrationBuilder.DropTable(
                name: "MonstersLoot");

            migrationBuilder.DropTable(
                name: "MonstersStats");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "CharactersBaseStats");
        }
    }
}
