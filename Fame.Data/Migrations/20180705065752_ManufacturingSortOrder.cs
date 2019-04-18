using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class ManufacturingSortOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManufacturingSortOrderId",
                table: "Component",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ManufacturingSortOrder",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturingSortOrder", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Component_ManufacturingSortOrderId",
                table: "Component",
                column: "ManufacturingSortOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Component_ManufacturingSortOrder_ManufacturingSortOrderId",
                table: "Component",
                column: "ManufacturingSortOrderId",
                principalTable: "ManufacturingSortOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Component_ManufacturingSortOrder_ManufacturingSortOrderId",
                table: "Component");

            migrationBuilder.DropTable(
                name: "ManufacturingSortOrder");

            migrationBuilder.DropIndex(
                name: "IX_Component_ManufacturingSortOrderId",
                table: "Component");

            migrationBuilder.DropColumn(
                name: "ManufacturingSortOrderId",
                table: "Component");
        }
    }
}
