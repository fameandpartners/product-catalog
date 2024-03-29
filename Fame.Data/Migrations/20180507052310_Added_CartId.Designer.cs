﻿// <auto-generated />
using Fame.Data;
using Fame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Fame.Data.Migrations
{
    [DbContext(typeof(FameContext))]
    [Migration("20180507052310_Added_CartId")]
    partial class Added_CartId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Fame.Data.Models.CompatibleOption", b =>
                {
                    b.Property<int>("CompatibleOptionId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDefault");

                    b.Property<int>("OptionId");

                    b.Property<int?>("ParentOptionId");

                    b.Property<int>("SectionId");

                    b.HasKey("CompatibleOptionId");

                    b.HasIndex("OptionId");

                    b.HasIndex("ParentOptionId");

                    b.HasIndex("SectionId", "OptionId", "ParentOptionId")
                        .IsUnique()
                        .HasFilter("[ParentOptionId] IS NOT NULL");

                    b.ToTable("CompatibleOption");
                });

            modelBuilder.Entity("Fame.Data.Models.Component", b =>
                {
                    b.Property<string>("ComponentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CartId");

                    b.Property<string>("ComponentTypeId");

                    b.Property<string>("RenderPositionId");

                    b.Property<string>("Title");

                    b.HasKey("ComponentId");

                    b.HasIndex("ComponentTypeId");

                    b.HasIndex("RenderPositionId");

                    b.ToTable("Component");
                });

            modelBuilder.Entity("Fame.Data.Models.ComponentType", b =>
                {
                    b.Property<string>("ComponentTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ComponentTypeCategory");

                    b.Property<bool>("IsProductCode");

                    b.Property<string>("ParentComponentTypeId");

                    b.Property<string>("SelectionTitle");

                    b.Property<string>("Title");

                    b.HasKey("ComponentTypeId");

                    b.HasIndex("ParentComponentTypeId");

                    b.ToTable("ComponentType");
                });

            modelBuilder.Entity("Fame.Data.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Order");

                    b.Property<int>("ProductVersionId");

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("GroupId");

                    b.HasIndex("ProductVersionId");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Fame.Data.Models.Incompatibility", b =>
                {
                    b.Property<int>("IncompatibilityId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IncompatibleOptionId");

                    b.Property<int>("OptionId");

                    b.Property<int?>("ParentOptionId");

                    b.HasKey("IncompatibilityId");

                    b.HasIndex("OptionId");

                    b.HasIndex("ParentOptionId");

                    b.HasIndex("IncompatibleOptionId", "OptionId", "ParentOptionId")
                        .IsUnique()
                        .HasFilter("[ParentOptionId] IS NOT NULL");

                    b.ToTable("Incompatibility");
                });

            modelBuilder.Entity("Fame.Data.Models.Option", b =>
                {
                    b.Property<int>("OptionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ComponentId");

                    b.Property<int>("ProductVersionId");

                    b.Property<string>("Title");

                    b.HasKey("OptionId");

                    b.HasIndex("ProductVersionId");

                    b.HasIndex("ComponentId", "ProductVersionId")
                        .IsUnique()
                        .HasFilter("[ComponentId] IS NOT NULL");

                    b.ToTable("Option");
                });

            modelBuilder.Entity("Fame.Data.Models.OptionPrice", b =>
                {
                    b.Property<int>("OptionId");

                    b.Property<string>("LocalisationCode");

                    b.Property<decimal>("Price");

                    b.HasKey("OptionId", "LocalisationCode");

                    b.ToTable("OptionPrice");
                });

            modelBuilder.Entity("Fame.Data.Models.OptionRenderComponent", b =>
                {
                    b.Property<string>("ComponentTypeId");

                    b.Property<int>("OptionId");

                    b.HasKey("ComponentTypeId", "OptionId");

                    b.HasIndex("OptionId");

                    b.ToTable("OptionRenderComponent");
                });

            modelBuilder.Entity("Fame.Data.Models.Product", b =>
                {
                    b.Property<string>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.HasKey("ProductId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Fame.Data.Models.ProductRenderComponent", b =>
                {
                    b.Property<string>("ComponentTypeId");

                    b.Property<int>("ProductVersionId");

                    b.HasKey("ComponentTypeId", "ProductVersionId");

                    b.HasIndex("ProductVersionId");

                    b.ToTable("ProductRenderComponent");
                });

            modelBuilder.Entity("Fame.Data.Models.ProductVersion", b =>
                {
                    b.Property<int>("ProductVersionId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("ProductId");

                    b.Property<int>("VersionState");

                    b.HasKey("ProductVersionId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductVersion");
                });

            modelBuilder.Entity("Fame.Data.Models.RenderPosition", b =>
                {
                    b.Property<string>("RenderPositionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Orientation");

                    b.Property<int>("Zoom");

                    b.HasKey("RenderPositionId");

                    b.ToTable("RenderPosition");
                });

            modelBuilder.Entity("Fame.Data.Models.Section", b =>
                {
                    b.Property<int>("SectionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ComponentTypeId");

                    b.Property<int>("Order");

                    b.Property<int>("SectionGroupId");

                    b.Property<int>("SelectionType");

                    b.Property<string>("Title");

                    b.HasKey("SectionId");

                    b.HasIndex("ComponentTypeId");

                    b.HasIndex("SectionGroupId");

                    b.ToTable("Section");
                });

            modelBuilder.Entity("Fame.Data.Models.SectionGroup", b =>
                {
                    b.Property<int>("SectionGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GroupId");

                    b.Property<int>("Order");

                    b.Property<string>("RenderPositionId");

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("SectionGroupId");

                    b.HasIndex("GroupId");

                    b.HasIndex("RenderPositionId");

                    b.ToTable("SectionGroup");
                });

            modelBuilder.Entity("Fame.Data.Models.CompatibleOption", b =>
                {
                    b.HasOne("Fame.Data.Models.Option", "Option")
                        .WithMany("CompatibleOptions")
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Option", "ParentOption")
                        .WithMany()
                        .HasForeignKey("ParentOptionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Section", "Section")
                        .WithMany("CompatibleOptions")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.Component", b =>
                {
                    b.HasOne("Fame.Data.Models.ComponentType", "ComponentType")
                        .WithMany()
                        .HasForeignKey("ComponentTypeId");

                    b.HasOne("Fame.Data.Models.RenderPosition", "RenderPosition")
                        .WithMany("Components")
                        .HasForeignKey("RenderPositionId");
                });

            modelBuilder.Entity("Fame.Data.Models.ComponentType", b =>
                {
                    b.HasOne("Fame.Data.Models.ComponentType", "ParentComponentType")
                        .WithMany("ChildComponentTypes")
                        .HasForeignKey("ParentComponentTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.Group", b =>
                {
                    b.HasOne("Fame.Data.Models.ProductVersion", "ProductVersion")
                        .WithMany("Groups")
                        .HasForeignKey("ProductVersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fame.Data.Models.Incompatibility", b =>
                {
                    b.HasOne("Fame.Data.Models.Option", "IncompatibleOption")
                        .WithMany("OptionsThatAreIncompatible")
                        .HasForeignKey("IncompatibleOptionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Option", "Option")
                        .WithMany("IncompatibleOptions")
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Option", "ParentOption")
                        .WithMany("IncompatibleParentOptions")
                        .HasForeignKey("ParentOptionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.Option", b =>
                {
                    b.HasOne("Fame.Data.Models.Component", "Component")
                        .WithMany()
                        .HasForeignKey("ComponentId");

                    b.HasOne("Fame.Data.Models.ProductVersion", "ProductVersion")
                        .WithMany("Options")
                        .HasForeignKey("ProductVersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fame.Data.Models.OptionPrice", b =>
                {
                    b.HasOne("Fame.Data.Models.Option", "Option")
                        .WithMany("Prices")
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.OptionRenderComponent", b =>
                {
                    b.HasOne("Fame.Data.Models.ComponentType", "ComponentType")
                        .WithMany("OptionRenderComponents")
                        .HasForeignKey("ComponentTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Option", "Option")
                        .WithMany("OptionRenderComponents")
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.ProductRenderComponent", b =>
                {
                    b.HasOne("Fame.Data.Models.ComponentType", "ComponentType")
                        .WithMany("ProductRenderComponents")
                        .HasForeignKey("ComponentTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.ProductVersion", "ProductVersion")
                        .WithMany("ProductRenderComponents")
                        .HasForeignKey("ProductVersionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.ProductVersion", b =>
                {
                    b.HasOne("Fame.Data.Models.Product", "Product")
                        .WithMany("ProductVersion")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Fame.Data.Models.Section", b =>
                {
                    b.HasOne("Fame.Data.Models.ComponentType", "ComponentType")
                        .WithMany("Sections")
                        .HasForeignKey("ComponentTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.SectionGroup", "SectionGroup")
                        .WithMany("Sections")
                        .HasForeignKey("SectionGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fame.Data.Models.SectionGroup", b =>
                {
                    b.HasOne("Fame.Data.Models.Group", "Group")
                        .WithMany("SectionGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fame.Data.Models.RenderPosition", "RenderPosition")
                        .WithMany("SectionGroups")
                        .HasForeignKey("RenderPositionId");
                });
#pragma warning restore 612, 618
        }
    }
}
