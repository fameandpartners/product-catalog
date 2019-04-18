using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class ComponentSortOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Component_ComponentType_ComponentTypeId",
                table: "Component");

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "Component",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Component_ComponentType_ComponentTypeId",
                table: "Component",
                column: "ComponentTypeId",
                principalTable: "ComponentType",
                principalColumn: "ComponentTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Component_ComponentType_ComponentTypeId",
                table: "Component");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Component");

            migrationBuilder.AddForeignKey(
                name: "FK_Component_ComponentType_ComponentTypeId",
                table: "Component",
                column: "ComponentTypeId",
                principalTable: "ComponentType",
                principalColumn: "ComponentTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
