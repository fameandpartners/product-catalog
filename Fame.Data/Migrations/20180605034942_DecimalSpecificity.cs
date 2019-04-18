using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class DecimalSpecificity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OptionPrice",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "SortWeightOther",
                table: "ComponentType",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "SortWeightDefault",
                table: "ComponentType",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OptionPrice",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SortWeightOther",
                table: "ComponentType",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SortWeightDefault",
                table: "ComponentType",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");
        }
    }
}
