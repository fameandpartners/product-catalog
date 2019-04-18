using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class SortWeightDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SortWeightOther",
                table: "ComponentType",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "SortWeightDefault",
                table: "ComponentType",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SortWeightOther",
                table: "ComponentType",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "SortWeightDefault",
                table: "ComponentType",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
