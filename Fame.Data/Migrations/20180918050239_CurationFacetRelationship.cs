using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class CurationFacetRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PrimarySilhouetteId",
                table: "Curation",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Curation_PrimarySilhouetteId",
                table: "Curation",
                column: "PrimarySilhouetteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Curation_Facet_PrimarySilhouetteId",
                table: "Curation",
                column: "PrimarySilhouetteId",
                principalTable: "Facet",
                principalColumn: "FacetId",
                onDelete: ReferentialAction.Restrict); // Changed from NoAction
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curation_Facet_PrimarySilhouetteId",
                table: "Curation");

            migrationBuilder.DropIndex(
                name: "IX_Curation_PrimarySilhouetteId",
                table: "Curation");

            migrationBuilder.AlterColumn<string>(
                name: "PrimarySilhouetteId",
                table: "Curation",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
