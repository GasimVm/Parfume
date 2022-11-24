using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class AddCardInCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Cards_CardId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CardId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CardId",
                table: "Customers",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Cards_CardId",
                table: "Customers",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Cards_CardId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CardId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CardId",
                table: "Users",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Cards_CardId",
                table: "Users",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
