using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class AddedBonusCardType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BonusCards");

            migrationBuilder.AddColumn<int>(
                name: "BonusCardTypeId",
                table: "BonusCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BonusCardTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusCardTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BonusCards_BonusCardTypeId",
                table: "BonusCards",
                column: "BonusCardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BonusCards_BonusCardTypes_BonusCardTypeId",
                table: "BonusCards",
                column: "BonusCardTypeId",
                principalTable: "BonusCardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BonusCards_BonusCardTypes_BonusCardTypeId",
                table: "BonusCards");

            migrationBuilder.DropTable(
                name: "BonusCardTypes");

            migrationBuilder.DropIndex(
                name: "IX_BonusCards_BonusCardTypeId",
                table: "BonusCards");

            migrationBuilder.DropColumn(
                name: "BonusCardTypeId",
                table: "BonusCards");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "BonusCards",
                type: "int",
                nullable: true);
        }
    }
}
