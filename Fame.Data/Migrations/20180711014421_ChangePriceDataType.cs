using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class ChangePriceDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OptionPrice");

            migrationBuilder.AddColumn<int>(
                name: "PriceInMinorUnits",
                table: "OptionPrice",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceInMinorUnits",
                table: "OptionPrice");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "OptionPrice",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
