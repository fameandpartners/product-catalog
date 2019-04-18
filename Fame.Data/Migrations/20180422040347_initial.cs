using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fame.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentType",
                columns: table => new
                {
                    ComponentTypeId = table.Column<string>(nullable: false),
                    ComponentTypeCategory = table.Column<int>(nullable: false),
                    IsProductCode = table.Column<bool>(nullable: false),
                    ParentComponentTypeId = table.Column<string>(nullable: true),
                    SelectionTitle = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentType", x => x.ComponentTypeId);
                    table.ForeignKey(
                        name: "FK_ComponentType_ComponentType_ParentComponentTypeId",
                        column: x => x.ParentComponentTypeId,
                        principalTable: "ComponentType",
                        principalColumn: "ComponentTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "RenderPosition",
                columns: table => new
                {
                    RenderPositionId = table.Column<string>(nullable: false),
                    Orientation = table.Column<int>(nullable: false),
                    Zoom = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenderPosition", x => x.RenderPositionId);
                });

            migrationBuilder.CreateTable(
                name: "ProductVersion",
                columns: table => new
                {
                    ProductVersionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ProductId = table.Column<string>(nullable: true),
                    VersionState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersion", x => x.ProductVersionId);
                    table.ForeignKey(
                        name: "FK_ProductVersion_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    ComponentId = table.Column<string>(nullable: false),
                    ComponentTypeId = table.Column<string>(nullable: true),
                    RenderPositionId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Component_ComponentType_ComponentTypeId",
                        column: x => x.ComponentTypeId,
                        principalTable: "ComponentType",
                        principalColumn: "ComponentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Component_RenderPosition_RenderPositionId",
                        column: x => x.RenderPositionId,
                        principalTable: "RenderPosition",
                        principalColumn: "RenderPositionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Order = table.Column<int>(nullable: false),
                    ProductVersionId = table.Column<int>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Group_ProductVersion_ProductVersionId",
                        column: x => x.ProductVersionId,
                        principalTable: "ProductVersion",
                        principalColumn: "ProductVersionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRenderComponent",
                columns: table => new
                {
                    ComponentTypeId = table.Column<string>(nullable: false),
                    ProductVersionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRenderComponent", x => new { x.ComponentTypeId, x.ProductVersionId });
                    table.ForeignKey(
                        name: "FK_ProductRenderComponent_ComponentType_ComponentTypeId",
                        column: x => x.ComponentTypeId,
                        principalTable: "ComponentType",
                        principalColumn: "ComponentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRenderComponent_ProductVersion_ProductVersionId",
                        column: x => x.ProductVersionId,
                        principalTable: "ProductVersion",
                        principalColumn: "ProductVersionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    OptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ComponentId = table.Column<string>(nullable: true),
                    ProductVersionId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_Option_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Option_ProductVersion_ProductVersionId",
                        column: x => x.ProductVersionId,
                        principalTable: "ProductVersion",
                        principalColumn: "ProductVersionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionGroup",
                columns: table => new
                {
                    SectionGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    RenderPositionId = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionGroup", x => x.SectionGroupId);
                    table.ForeignKey(
                        name: "FK_SectionGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionGroup_RenderPosition_RenderPositionId",
                        column: x => x.RenderPositionId,
                        principalTable: "RenderPosition",
                        principalColumn: "RenderPositionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Incompatibility",
                columns: table => new
                {
                    IncompatibilityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IncompatibleOptionId = table.Column<int>(nullable: false),
                    OptionId = table.Column<int>(nullable: false),
                    ParentOptionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incompatibility", x => x.IncompatibilityId);
                    table.ForeignKey(
                        name: "FK_Incompatibility_Option_IncompatibleOptionId",
                        column: x => x.IncompatibleOptionId,
                        principalTable: "Option",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incompatibility_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incompatibility_Option_ParentOptionId",
                        column: x => x.ParentOptionId,
                        principalTable: "Option",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OptionPrice",
                columns: table => new
                {
                    OptionId = table.Column<int>(nullable: false),
                    LocalisationCode = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionPrice", x => new { x.OptionId, x.LocalisationCode });
                    table.ForeignKey(
                        name: "FK_OptionPrice_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OptionRenderComponent",
                columns: table => new
                {
                    ComponentTypeId = table.Column<string>(nullable: false),
                    OptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionRenderComponent", x => new { x.ComponentTypeId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_OptionRenderComponent_ComponentType_ComponentTypeId",
                        column: x => x.ComponentTypeId,
                        principalTable: "ComponentType",
                        principalColumn: "ComponentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OptionRenderComponent_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    SectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ComponentTypeId = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    SectionGroupId = table.Column<int>(nullable: false),
                    SelectionType = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.SectionId);
                    table.ForeignKey(
                        name: "FK_Section_ComponentType_ComponentTypeId",
                        column: x => x.ComponentTypeId,
                        principalTable: "ComponentType",
                        principalColumn: "ComponentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Section_SectionGroup_SectionGroupId",
                        column: x => x.SectionGroupId,
                        principalTable: "SectionGroup",
                        principalColumn: "SectionGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompatibleOption",
                columns: table => new
                {
                    CompatibleOptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDefault = table.Column<bool>(nullable: false),
                    OptionId = table.Column<int>(nullable: false),
                    ParentOptionId = table.Column<int>(nullable: true),
                    SectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompatibleOption", x => x.CompatibleOptionId);
                    table.ForeignKey(
                        name: "FK_CompatibleOption_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompatibleOption_Option_ParentOptionId",
                        column: x => x.ParentOptionId,
                        principalTable: "Option",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompatibleOption_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompatibleOption_OptionId",
                table: "CompatibleOption",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompatibleOption_ParentOptionId",
                table: "CompatibleOption",
                column: "ParentOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompatibleOption_SectionId_OptionId_ParentOptionId",
                table: "CompatibleOption",
                columns: new[] { "SectionId", "OptionId", "ParentOptionId" },
                unique: true,
                filter: "[ParentOptionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Component_ComponentTypeId",
                table: "Component",
                column: "ComponentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Component_RenderPositionId",
                table: "Component",
                column: "RenderPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentType_ParentComponentTypeId",
                table: "ComponentType",
                column: "ParentComponentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ProductVersionId",
                table: "Group",
                column: "ProductVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Incompatibility_OptionId",
                table: "Incompatibility",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Incompatibility_ParentOptionId",
                table: "Incompatibility",
                column: "ParentOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Incompatibility_IncompatibleOptionId_OptionId_ParentOptionId",
                table: "Incompatibility",
                columns: new[] { "IncompatibleOptionId", "OptionId", "ParentOptionId" },
                unique: true,
                filter: "[ParentOptionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Option_ProductVersionId",
                table: "Option",
                column: "ProductVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_ComponentId_ProductVersionId",
                table: "Option",
                columns: new[] { "ComponentId", "ProductVersionId" },
                unique: true,
                filter: "[ComponentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OptionRenderComponent_OptionId",
                table: "OptionRenderComponent",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRenderComponent_ProductVersionId",
                table: "ProductRenderComponent",
                column: "ProductVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersion_ProductId",
                table: "ProductVersion",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_ComponentTypeId",
                table: "Section",
                column: "ComponentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_SectionGroupId",
                table: "Section",
                column: "SectionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionGroup_GroupId",
                table: "SectionGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionGroup_RenderPositionId",
                table: "SectionGroup",
                column: "RenderPositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompatibleOption");

            migrationBuilder.DropTable(
                name: "Incompatibility");

            migrationBuilder.DropTable(
                name: "OptionPrice");

            migrationBuilder.DropTable(
                name: "OptionRenderComponent");

            migrationBuilder.DropTable(
                name: "ProductRenderComponent");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "SectionGroup");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "ComponentType");

            migrationBuilder.DropTable(
                name: "RenderPosition");

            migrationBuilder.DropTable(
                name: "ProductVersion");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
