using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class ComponentMeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentMeta",
                columns: table => new
                {
                    ComponentId = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentMeta", x => new { x.ComponentId, x.Key });
                    table.ForeignKey(
                        name: "FK_ComponentMeta_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentMeta");
        }
    }
}
