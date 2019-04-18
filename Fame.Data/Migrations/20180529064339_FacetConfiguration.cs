using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class FacetConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "FacetGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Facet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FacetCategory",
                columns: table => new
                {
                    FacetCategoryId = table.Column<string>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacetCategory", x => x.FacetCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "FacetConfiguration",
                columns: table => new
                {
                    FacetConfigurationId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacetConfiguration", x => x.FacetConfigurationId);
                });

            migrationBuilder.CreateTable(
                name: "FacetCategoryGroup",
                columns: table => new
                {
                    FacetCategoryId = table.Column<string>(nullable: false),
                    FacetGroupId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacetCategoryGroup", x => new { x.FacetCategoryId, x.FacetGroupId });
                    table.ForeignKey(
                        name: "FK_FacetCategoryGroup_FacetCategory_FacetCategoryId",
                        column: x => x.FacetCategoryId,
                        principalTable: "FacetCategory",
                        principalColumn: "FacetCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacetCategoryGroup_FacetGroup_FacetGroupId",
                        column: x => x.FacetGroupId,
                        principalTable: "FacetGroup",
                        principalColumn: "FacetGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacetCategoryConfiguration",
                columns: table => new
                {
                    FacetCategoryId = table.Column<string>(nullable: false),
                    FacetConfigurationId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacetCategoryConfiguration", x => new { x.FacetCategoryId, x.FacetConfigurationId });
                    table.ForeignKey(
                        name: "FK_FacetCategoryConfiguration_FacetCategory_FacetCategoryId",
                        column: x => x.FacetCategoryId,
                        principalTable: "FacetCategory",
                        principalColumn: "FacetCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacetCategoryConfiguration_FacetConfiguration_FacetConfigurationId",
                        column: x => x.FacetConfigurationId,
                        principalTable: "FacetConfiguration",
                        principalColumn: "FacetConfigurationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacetCategoryConfiguration_FacetConfigurationId",
                table: "FacetCategoryConfiguration",
                column: "FacetConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_FacetCategoryGroup_FacetGroupId",
                table: "FacetCategoryGroup",
                column: "FacetGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacetCategoryConfiguration");

            migrationBuilder.DropTable(
                name: "FacetCategoryGroup");

            migrationBuilder.DropTable(
                name: "FacetConfiguration");

            migrationBuilder.DropTable(
                name: "FacetCategory");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "FacetGroup");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Facet");
        }
    }
}
