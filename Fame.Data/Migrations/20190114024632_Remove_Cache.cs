using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class Remove_Cache : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cache",
                schema: "Cache");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Cache");

            migrationBuilder.CreateTable(
                name: "Cache",
                schema: "Cache",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 449, nullable: false),
                    AbsoluteExpiration = table.Column<DateTimeOffset>(nullable: true),
                    ExpiresAtTime = table.Column<DateTimeOffset>(nullable: false),
                    SlidingExpirationInSeconds = table.Column<long>(nullable: true),
                    Value = table.Column<byte[]>(nullable: false)
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
    }
}
