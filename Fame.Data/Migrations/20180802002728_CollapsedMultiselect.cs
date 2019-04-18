using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class CollapsedMultiselect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Collapsed",
                table: "FacetGroup",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Multiselect",
                table: "FacetGroup",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Collapsed",
                table: "FacetGroup");

            migrationBuilder.DropColumn(
                name: "Multiselect",
                table: "FacetGroup");
        }
    }
}
