using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class AddCurationProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "Curation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Curation_ProductId",
                table: "Curation",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Curation_Product_ProductId",
                table: "Curation",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curation_Product_ProductId",
                table: "Curation");

            migrationBuilder.DropIndex(
                name: "IX_Curation_ProductId",
                table: "Curation");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Curation");
        }
    }
}
