using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class AddFacetMeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacetMeta",
                columns: table => new
                {
                    FacetId = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacetMeta", x => new { x.FacetId, x.Key });
                    table.ForeignKey(
                        name: "FK_FacetMeta_Facet_FacetId",
                        column: x => x.FacetId,
                        principalTable: "Facet",
                        principalColumn: "FacetId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacetMeta");
        }
    }
}
