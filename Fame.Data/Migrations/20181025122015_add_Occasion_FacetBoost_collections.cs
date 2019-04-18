using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class add_Occasion_FacetBoost_collections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectionFacetBoost",
                columns: table => new
                {
                    FacetBoostId = table.Column<int>(nullable: false),
                    CollectionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionFacetBoost", x => new { x.CollectionId, x.FacetBoostId });
                    table.ForeignKey(
                        name: "FK_CollectionFacetBoost_Collection_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collection",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionFacetBoost_FacetBoost_FacetBoostId",
                        column: x => x.FacetBoostId,
                        principalTable: "FacetBoost",
                        principalColumn: "FacetBoostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CollectionOccasion",
                columns: table => new
                {
                    OccasionId = table.Column<string>(nullable: false),
                    CollectionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionOccasion", x => new { x.CollectionId, x.OccasionId });
                    table.ForeignKey(
                        name: "FK_CollectionOccasion_Collection_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collection",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionOccasion_Occasion_OccasionId",
                        column: x => x.OccasionId,
                        principalTable: "Occasion",
                        principalColumn: "OccasionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionFacetBoost_FacetBoostId",
                table: "CollectionFacetBoost",
                column: "FacetBoostId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionOccasion_OccasionId",
                table: "CollectionOccasion",
                column: "OccasionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionFacetBoost");

            migrationBuilder.DropTable(
                name: "CollectionOccasion");
        }
    }
}
