using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class change_workflow_datetimeoffset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TriggeredDateTime",
                table: "Workflow",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TriggeredDateTime",
                table: "Workflow",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }
    }
}
