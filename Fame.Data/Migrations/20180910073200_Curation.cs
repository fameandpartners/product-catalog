using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class Curation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Curation",
                columns: table => new
                {
                    PID = table.Column<string>(nullable: false),
                    ProductDocumentVersionId = table.Column<string>(nullable: true),
                    ProductDocumentId = table.Column<string>(nullable: true),
                    PrimarySilhouetteId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curation", x => x.PID);
                });

            migrationBuilder.CreateTable(
                name: "CurationMedia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullPath = table.Column<string>(nullable: true),
                    PID = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurationMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurationMedia_Curation_PID",
                        column: x => x.PID,
                        principalTable: "Curation",
                        principalColumn: "PID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurationMedia_PID",
                table: "CurationMedia",
                column: "PID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurationMedia");

            migrationBuilder.DropTable(
                name: "Curation");
        }
    }
}
