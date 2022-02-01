using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class AddTablePaymentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentHistoryId",
                table: "CrediteHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrediteHistories_PaymentHistoryId",
                table: "CrediteHistories",
                column: "PaymentHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CrediteHistories_PaymentHistory_PaymentHistoryId",
                table: "CrediteHistories",
                column: "PaymentHistoryId",
                principalTable: "PaymentHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrediteHistories_PaymentHistory_PaymentHistoryId",
                table: "CrediteHistories");

            migrationBuilder.DropIndex(
                name: "IX_CrediteHistories_PaymentHistoryId",
                table: "CrediteHistories");

            migrationBuilder.DropColumn(
                name: "PaymentHistoryId",
                table: "CrediteHistories");
        }
    }
}
