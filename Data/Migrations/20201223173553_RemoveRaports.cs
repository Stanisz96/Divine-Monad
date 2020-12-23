using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class RemoveRaports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attacker",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Crit = table.Column<bool>(type: "bit", nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: false),
                    HP = table.Column<int>(type: "int", nullable: false),
                    Miss = table.Column<bool>(type: "bit", nullable: false),
                    attackerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attacker", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Defender",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Block = table.Column<bool>(type: "bit", nullable: false),
                    HP = table.Column<int>(type: "int", nullable: false),
                    Receive = table.Column<int>(type: "int", nullable: false),
                    defenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defender", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Opponent",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    opponentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opponent", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    playerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Raports",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPvp = table.Column<bool>(type: "bit", nullable: false),
                    OpponentID = table.Column<int>(type: "int", nullable: true),
                    PlayerID = table.Column<int>(type: "int", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttackerID = table.Column<int>(type: "int", nullable: true),
                    DefenderID = table.Column<int>(type: "int", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    RaportGeneratorID = table.Column<int>(type: "int", nullable: true)
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
    }
}
