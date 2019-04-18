using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class Add_SqlSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SQLSessions",
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
                    table.PrimaryKey("PK_SQLSessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "Index_ExpiresAtTime",
                schema: "Cache",
                table: "SQLSessions",
                column: "ExpiresAtTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SQLSessions",
                schema: "Cache");
        }
    }
}
