using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class AddReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferencesCustomerId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReferencesId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ReferencesCustomerId",
                table: "Customers",
                column: "ReferencesCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Customers_ReferencesCustomerId",
                table: "Customers",
                column: "ReferencesCustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Customers_ReferencesCustomerId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ReferencesCustomerId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ReferencesCustomerId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ReferencesId",
                table: "Customers");
        }
    }
}
