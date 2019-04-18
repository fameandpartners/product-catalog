using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class AddCurationComponents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurationComponent",
                columns: table => new
                {
                    PID = table.Column<string>(nullable: false),
                    ComponentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurationComponent", x => new { x.PID, x.ComponentId });
                    table.ForeignKey(
                        name: "FK_CurationComponent_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurationComponent_Curation_PID",
                        column: x => x.PID,
                        principalTable: "Curation",
                        principalColumn: "PID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurationComponent_ComponentId",
                table: "CurationComponent",
                column: "ComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurationComponent");
        }
    }
}
