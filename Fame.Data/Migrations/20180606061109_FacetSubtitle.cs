using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class FacetSubtitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subtitle",
                table: "FacetGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subtitle",
                table: "Facet",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subtitle",
                table: "FacetGroup");

            migrationBuilder.DropColumn(
                name: "Subtitle",
                table: "Facet");
        }
    }
}
