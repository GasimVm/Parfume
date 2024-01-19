using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class AddedBonusCardHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BonusCardAmount",
                table: "Orders",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Balans",
                table: "BonusCards",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BonusCardHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    BonusCardId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusCardHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonusCardHistories_BonusCards_BonusCardId",
                        column: x => x.BonusCardId,
                        principalTable: "BonusCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BonusCardHistories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonusCardHistories_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BonusCardHistories_BonusCardId",
                table: "BonusCardHistories",
                column: "BonusCardId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusCardHistories_CustomerId",
                table: "BonusCardHistories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusCardHistories_OrderId",
                table: "BonusCardHistories",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusCardHistories");

            migrationBuilder.DropColumn(
                name: "BonusCardAmount",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "Balans",
                table: "BonusCards",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
