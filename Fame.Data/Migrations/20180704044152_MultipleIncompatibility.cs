using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class MultipleIncompatibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incompatibility_Option_IncompatibleOptionId",
                table: "Incompatibility");

            migrationBuilder.DropIndex(
                name: "IX_Incompatibility_IncompatibleOptionId_OptionId_ParentOptionId",
                table: "Incompatibility");

            migrationBuilder.DropColumn(
                name: "IncompatibleOptionId",
                table: "Incompatibility");

            migrationBuilder.CreateTable(
                name: "IncompatibleOption",
                columns: table => new
                {
                    IncompatibilityId = table.Column<int>(nullable: false),
                    OptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncompatibleOption", x => new { x.IncompatibilityId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_IncompatibleOption_Incompatibility_IncompatibilityId",
                        column: x => x.IncompatibilityId,
                        principalTable: "Incompatibility",
                        principalColumn: "IncompatibilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncompatibleOption_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncompatibleOption_OptionId",
                table: "IncompatibleOption",
                column: "OptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncompatibleOption");

            migrationBuilder.AddColumn<int>(
                name: "IncompatibleOptionId",
                table: "Incompatibility",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Incompatibility_IncompatibleOptionId_OptionId_ParentOptionId",
                table: "Incompatibility",
                columns: new[] { "IncompatibleOptionId", "OptionId", "ParentOptionId" },
                unique: true,
                filter: "[ParentOptionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Incompatibility_Option_IncompatibleOptionId",
                table: "Incompatibility",
                column: "IncompatibleOptionId",
                principalTable: "Option",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
