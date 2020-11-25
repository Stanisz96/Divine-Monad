using Microsoft.EntityFrameworkCore.Migrations;

namespace DivineMonad.Data.Migrations
{
    public partial class UpdateItemsAndCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCategories_Rarity_RarityId",
                table: "ItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_ItemCategories_RarityId",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "RarityId",
                table: "ItemCategories");

            migrationBuilder.AddColumn<int>(
                name: "RarityId",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_Items_RarityId",
                table: "Items",
                column: "RarityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Rarity_RarityId",
                table: "Items",
                column: "RarityId",
                principalTable: "Rarity",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Rarity_RarityId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_RarityId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RarityId",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "RarityId",
                table: "ItemCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

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
    }
}
