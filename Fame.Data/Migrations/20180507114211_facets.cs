using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class facets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facet",
                columns: table => new
                {
                    FacetId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facet", x => x.FacetId);
                });

            migrationBuilder.CreateTable(
                name: "ComponentFacet",
                columns: table => new
                {
                    ComponentId = table.Column<string>(nullable: false),
                    FacetId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentFacet", x => new { x.ComponentId, x.FacetId });
                    table.ForeignKey(
                        name: "FK_ComponentFacet_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentFacet_Facet_FacetId",
                        column: x => x.FacetId,
                        principalTable: "Facet",
                        principalColumn: "FacetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentFacet_FacetId",
                table: "ComponentFacet",
                column: "FacetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentFacet");

            migrationBuilder.DropTable(
                name: "Facet");
        }
    }
}
