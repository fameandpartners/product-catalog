using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class AddNameAndDescriptionToCuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Curation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Curation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Curation");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Curation");
        }
    }
}
