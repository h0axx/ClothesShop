using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShop.Data.Migrations
{
    public partial class basketmigrationrepair3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "BasketItems");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "BasketItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BasketItems");

            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "BasketItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
