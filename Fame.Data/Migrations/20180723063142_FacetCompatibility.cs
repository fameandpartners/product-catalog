using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class FacetCompatibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentFacet");

            migrationBuilder.AddColumn<string>(
                name: "CompatibilityRule",
                table: "Facet",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompatibilityRule",
                table: "Facet");

            migrationBuilder.CreateTable(
                name: "ComponentFacet",
                columns: table => new
                {
                    ComponentId = table.Column<string>(nullable: false),
                    FacetId = table.Column<string>(nullable: false),
                    CollectionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentFacet", x => new { x.ComponentId, x.FacetId, x.CollectionId });
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
    }
}
