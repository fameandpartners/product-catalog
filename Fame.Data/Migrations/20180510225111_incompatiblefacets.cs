using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fame.Data.Migrations
{
    public partial class incompatiblefacets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComponentFacet",
                table: "ComponentFacet");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "ComponentFacet",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComponentFacet",
                table: "ComponentFacet",
                columns: new[] { "ComponentId", "FacetId", "GroupId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncompatibleFacet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComponentFacet",
                table: "ComponentFacet");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "ComponentFacet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComponentFacet",
                table: "ComponentFacet",
                columns: new[] { "ComponentId", "FacetId" });
        }
    }
}
