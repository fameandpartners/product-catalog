using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class remove_incompatiblefacet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncompatibleFacet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncompatibleFacet",
                columns: table => new
                {
                    FacetId = table.Column<string>(nullable: false),
                    IncompatibilityId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncompatibleFacet", x => new { x.FacetId, x.IncompatibilityId });
                    table.ForeignKey(
                        name: "FK_IncompatibleFacet_Facet_FacetId",
                        column: x => x.FacetId,
                        principalTable: "Facet",
                        principalColumn: "FacetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncompatibleFacet_Facet_IncompatibilityId",
                        column: x => x.IncompatibilityId,
                        principalTable: "Facet",
                        principalColumn: "FacetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncompatibleFacet_IncompatibilityId",
                table: "IncompatibleFacet",
                column: "IncompatibilityId");
        }
    }
}
