using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class Add_Collection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collection",
                columns: table => new
                {
                    CollectionId = table.Column<string>(nullable: false),
                    CollectionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection", x => x.CollectionId);
                });

            migrationBuilder.CreateTable(
                name: "CollectionFacet",
                columns: table => new
                {
                    FacetId = table.Column<string>(nullable: false),
                    CollectionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionFacet", x => new { x.CollectionId, x.FacetId });
                    table.ForeignKey(
                        name: "FK_CollectionFacet_Collection_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collection",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionFacet_Facet_FacetId",
                        column: x => x.FacetId,
                        principalTable: "Facet",
                        principalColumn: "FacetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CollectionProduct",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    CollectionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionProduct", x => new { x.CollectionId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CollectionProduct_Collection_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collection",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionFacet_FacetId",
                table: "CollectionFacet",
                column: "FacetId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionProduct_ProductId",
                table: "CollectionProduct",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionFacet");

            migrationBuilder.DropTable(
                name: "CollectionProduct");

            migrationBuilder.DropTable(
                name: "Collection");
        }
    }
}
