using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class AddCardVirtualTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Cards_CardId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Cards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardId",
                table: "Cards",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Cards_CardId",
                table: "Cards",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
