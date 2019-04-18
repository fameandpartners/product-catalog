using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class ProductNameAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductNameOrder",
                table: "FacetGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Facet",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Facet",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagPriority",
                table: "Facet",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductNameOrder",
                table: "FacetGroup");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Facet");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Facet");

            migrationBuilder.DropColumn(
                name: "TagPriority",
                table: "Facet");
        }
    }
}
