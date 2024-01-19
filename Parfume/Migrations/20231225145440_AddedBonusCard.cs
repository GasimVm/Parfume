using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class AddedBonusCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BonusCardId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PayByBonusCard",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BonusCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Balans = table.Column<int>(type: "int", nullable: true),
                    CardNumber = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonusCards_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BonusCardId",
                table: "Orders",
                column: "BonusCardId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusCards_CustomerId",
                table: "BonusCards",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_BonusCards_BonusCardId",
                table: "Orders",
                column: "BonusCardId",
                principalTable: "BonusCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_BonusCards_BonusCardId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "BonusCards");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BonusCardId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BonusCardId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PayByBonusCard",
                table: "Orders");
        }
    }
}
