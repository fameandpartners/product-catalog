using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class ProductVerionPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductVersionPrice",
                columns: table => new
                {
                    ProductVersionId = table.Column<int>(nullable: false),
                    LocalisationCode = table.Column<string>(nullable: false),
                    PriceInMinorUnits = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersionPrice", x => new { x.ProductVersionId, x.LocalisationCode });
                    table.ForeignKey(
                        name: "FK_ProductVersionPrice_ProductVersion_ProductVersionId",
                        column: x => x.ProductVersionId,
                        principalTable: "ProductVersion",
                        principalColumn: "ProductVersionId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVersionPrice");
        }
    }
}
