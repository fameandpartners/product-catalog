using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class AddOccasion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Occasion",
                columns: table => new
                {
                    OccasionId = table.Column<string>(nullable: false),
                    OccasionName = table.Column<string>(nullable: true),
                    ComponentCompatibilityRule = table.Column<string>(nullable: true),
                    FacetCompatibilityRule = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occasion", x => x.OccasionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Occasion");
        }
    }
}
