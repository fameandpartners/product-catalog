using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class AddCurationVariation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "CurationMedia");

            migrationBuilder.DropColumn(
                name: "VariationId",
                table: "CurationMedia");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "CurationMedia");

            migrationBuilder.RenameColumn(
                name: "FullPath",
                table: "CurationMedia",
                newName: "SizeDescription");

            migrationBuilder.AddColumn<string>(
                name: "FitDescription",
                table: "CurationMedia",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "CurationMedia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CurationMediaVariant",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurationMediaId = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Ext = table.Column<string>(nullable: true),
                    IsOriginal = table.Column<bool>(nullable: false),
                    Quality = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurationMediaVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurationMediaVariant_CurationMedia_CurationMediaId",
                        column: x => x.CurationMediaId,
                        principalTable: "CurationMedia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurationMediaVariant_CurationMediaId",
                table: "CurationMediaVariant",
                column: "CurationMediaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurationMediaVariant");

            migrationBuilder.DropColumn(
                name: "FitDescription",
                table: "CurationMedia");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CurationMedia");

            migrationBuilder.RenameColumn(
                name: "SizeDescription",
                table: "CurationMedia",
                newName: "FullPath");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "CurationMedia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "VariationId",
                table: "CurationMedia",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "CurationMedia",
                nullable: false,
                defaultValue: 0);
        }
    }
}
