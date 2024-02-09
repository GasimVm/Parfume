using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class AddedCardIdInPaymentHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "PaymentHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistories_CardId",
                table: "PaymentHistories",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistories_Cards_CardId",
                table: "PaymentHistories",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistories_Cards_CardId",
                table: "PaymentHistories");

            migrationBuilder.DropIndex(
                name: "IX_PaymentHistories_CardId",
                table: "PaymentHistories");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "PaymentHistories");
        }
    }
}
