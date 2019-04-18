using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class ReCreateCacheTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Cache");

            migrationBuilder.CreateTable(
                name: "Cache",
                schema: "Cache",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 449, nullable: false),
                    Value = table.Column<byte[]>(nullable: false),
                    ExpiresAtTime = table.Column<DateTimeOffset>(nullable: false),
                    SlidingExpirationInSeconds = table.Column<long>(nullable: true),
                    AbsoluteExpiration = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cache", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "Index_ExpiresAtTime",
                schema: "Cache",
                table: "Cache",
                column: "ExpiresAtTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cache",
                schema: "Cache");
        }
    }
}
