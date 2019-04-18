using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class facet_group : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacetGroupId",
                table: "Facet",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FacetGroup",
                columns: table => new
                {
                    FacetGroupId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacetGroup", x => x.FacetGroupId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facet_FacetGroupId",
                table: "Facet",
                column: "FacetGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facet_FacetGroup_FacetGroupId",
                table: "Facet",
                column: "FacetGroupId",
                principalTable: "FacetGroup",
                principalColumn: "FacetGroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facet_FacetGroup_FacetGroupId",
                table: "Facet");

            migrationBuilder.DropTable(
                name: "FacetGroup");

            migrationBuilder.DropIndex(
                name: "IX_Facet_FacetGroupId",
                table: "Facet");

            migrationBuilder.DropColumn(
                name: "FacetGroupId",
                table: "Facet");
        }
    }
}
