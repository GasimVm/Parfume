using Microsoft.EntityFrameworkCore.Migrations;

namespace Parfume.Migrations
{
    public partial class NewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FirstPrice",
                table: "Orders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BlockNote",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BlockNote",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Customers");
        }
    }
}
