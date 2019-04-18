using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class Rename_Workflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workflow",
                columns: table => new
                {
                    WorkflowStep = table.Column<int>(nullable: false),
                    TriggeredDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflow", x => x.WorkflowStep);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workflow");
        }
    }
}
