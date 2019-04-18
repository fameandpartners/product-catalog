using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class RemoveSQLSessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SQLSessions",
                schema: "Cache");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SQLSessions",
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
                    table.PrimaryKey("PK_SQLSessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "Index_ExpiresAtTime",
                schema: "Cache",
                table: "SQLSessions",
                column: "ExpiresAtTime");
        }
    }
}
