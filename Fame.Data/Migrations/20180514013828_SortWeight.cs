using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class SortWeight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SortWeightDefault",
                table: "ComponentType",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SortWeightOther",
                table: "ComponentType",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortWeightDefault",
                table: "ComponentType");

            migrationBuilder.DropColumn(
                name: "SortWeightOther",
                table: "ComponentType");
        }
    }
}
