using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class IndexAggregationFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAggregatedFacet",
                table: "FacetGroup",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AggregateOnIndex",
                table: "ComponentType",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAggregatedFacet",
                table: "FacetGroup");

            migrationBuilder.DropColumn(
                name: "AggregateOnIndex",
                table: "ComponentType");
        }
    }
}
