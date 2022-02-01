using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class AddTableChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrediteHistories_PaymentHistory_PaymentHistoryId",
                table: "CrediteHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_Customers_CustomerId",
                table: "PaymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_Orders_OrderId",
                table: "PaymentHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentHistory",
                table: "PaymentHistory");

            migrationBuilder.RenameTable(
                name: "PaymentHistory",
                newName: "PaymentHistories");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentHistory_OrderId",
                table: "PaymentHistories",
                newName: "IX_PaymentHistories_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentHistory_CustomerId",
                table: "PaymentHistories",
                newName: "IX_PaymentHistories_CustomerId");

            migrationBuilder.AlterColumn<double>(
                name: "MonthPrice",
                table: "Orders",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(25)",
                oldPrecision: 25,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentHistories",
                table: "PaymentHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CrediteHistories_PaymentHistories_PaymentHistoryId",
                table: "CrediteHistories",
                column: "PaymentHistoryId",
                principalTable: "PaymentHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistories_Customers_CustomerId",
                table: "PaymentHistories",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistories_Orders_OrderId",
                table: "PaymentHistories",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrediteHistories_PaymentHistories_PaymentHistoryId",
                table: "CrediteHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistories_Customers_CustomerId",
                table: "PaymentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistories_Orders_OrderId",
                table: "PaymentHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentHistories",
                table: "PaymentHistories");

            migrationBuilder.RenameTable(
                name: "PaymentHistories",
                newName: "PaymentHistory");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentHistories_OrderId",
                table: "PaymentHistory",
                newName: "IX_PaymentHistory_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentHistories_CustomerId",
                table: "PaymentHistory",
                newName: "IX_PaymentHistory_CustomerId");

            migrationBuilder.AlterColumn<double>(
                name: "MonthPrice",
                table: "Orders",
                type: "float(25)",
                precision: 25,
                scale: 4,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentHistory",
                table: "PaymentHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CrediteHistories_PaymentHistory_PaymentHistoryId",
                table: "CrediteHistories",
                column: "PaymentHistoryId",
                principalTable: "PaymentHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_Customers_CustomerId",
                table: "PaymentHistory",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_Orders_OrderId",
                table: "PaymentHistory",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
