using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class ChangeBonus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BonusHistories_Bonus_BonusId",
                table: "BonusHistories");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Customers_Customers_ReferencesId",
            //    table: "Customers");

            //migrationBuilder.DropIndex(
            //    name: "IX_Customers_ReferencesId",
            //    table: "Customers");

            //migrationBuilder.AddColumn<int>(
            //    name: "ReferencesCustomerId",
            //    table: "Customers",
            //    type: "int",
            //    nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BonusId",
                table: "BonusHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Customers_ReferencesCustomerId",
            //    table: "Customers",
            //    column: "ReferencesCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BonusHistories_Bonus_BonusId",
                table: "BonusHistories",
                column: "BonusId",
                principalTable: "Bonus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Customers_Customers_ReferencesCustomerId",
            //    table: "Customers",
            //    column: "ReferencesCustomerId",
            //    principalTable: "Customers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BonusHistories_Bonus_BonusId",
                table: "BonusHistories");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Customers_Customers_ReferencesCustomerId",
            //    table: "Customers");

            //migrationBuilder.DropIndex(
            //    name: "IX_Customers_ReferencesCustomerId",
            //    table: "Customers");

            //migrationBuilder.DropColumn(
            //    name: "ReferencesCustomerId",
            //    table: "Customers");

            migrationBuilder.AlterColumn<int>(
                name: "BonusId",
                table: "BonusHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Customers_ReferencesId",
            //    table: "Customers",
            //    column: "ReferencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_BonusHistories_Bonus_BonusId",
                table: "BonusHistories",
                column: "BonusId",
                principalTable: "Bonus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Customers_Customers_ReferencesId",
            //    table: "Customers",
            //    column: "ReferencesId",
            //    principalTable: "Customers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
