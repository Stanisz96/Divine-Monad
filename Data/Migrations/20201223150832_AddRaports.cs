using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class AddRaports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attacker",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    attackerId = table.Column<int>(nullable: false),
                    HP = table.Column<int>(nullable: false),
                    Crit = table.Column<bool>(nullable: false),
                    Miss = table.Column<bool>(nullable: false),
                    Damage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attacker", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Defender",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    defenderId = table.Column<int>(nullable: false),
                    HP = table.Column<int>(nullable: false),
                    Block = table.Column<bool>(nullable: false),
                    Receive = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defender", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Opponent",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    opponentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opponent", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    playerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Raports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerID = table.Column<int>(nullable: true),
                    OpponentID = table.Column<int>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    IsPvp = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Raports_Opponent_OpponentID",
                        column: x => x.OpponentID,
                        principalTable: "Opponent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Raports_Player_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Player",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Round",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    AttackerID = table.Column<int>(nullable: true),
                    DefenderID = table.Column<int>(nullable: true),
                    RaportGeneratorID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Round_Attacker_AttackerID",
                        column: x => x.AttackerID,
                        principalTable: "Attacker",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Round_Defender_DefenderID",
                        column: x => x.DefenderID,
                        principalTable: "Defender",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Round_Raports_RaportGeneratorID",
                        column: x => x.RaportGeneratorID,
                        principalTable: "Raports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Raports_OpponentID",
                table: "Raports",
                column: "OpponentID");

            migrationBuilder.CreateIndex(
                name: "IX_Raports_PlayerID",
                table: "Raports",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Round_AttackerID",
                table: "Round",
                column: "AttackerID");

            migrationBuilder.CreateIndex(
                name: "IX_Round_DefenderID",
                table: "Round",
                column: "DefenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Round_RaportGeneratorID",
                table: "Round",
                column: "RaportGeneratorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Round");

            migrationBuilder.DropTable(
                name: "Attacker");

            migrationBuilder.DropTable(
                name: "Defender");

            migrationBuilder.DropTable(
                name: "Raports");

            migrationBuilder.DropTable(
                name: "Opponent");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
