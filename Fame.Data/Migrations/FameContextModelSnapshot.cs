﻿// <auto-generated />
using System;
using Fame.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fame.Data.Migrations
{
    [DbContext(typeof(FameContext))]
    partial class FameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Fame.Data.Models.Collection", b =>
                {
                    b.Property<string>("CollectionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CollectionName");

                    b.HasKey("CollectionId");

                    b.ToTable("Collection");
                });

            modelBuilder.Entity("Fame.Data.Models.CollectionFacet", b =>
                {
                    b.Property<string>("CollectionId");

                    b.Property<string>("FacetId");

                    b.HasKey("CollectionId", "FacetId");

                    b.HasIndex("FacetId");

                    b.ToTable("CollectionFacet");
                });

            modelBuilder.Entity("Fame.Data.Models.CollectionFacetBoost", b =>
                {
                    b.Property<string>("CollectionId");

                    b.Property<int>("FacetBoostId");

                    b.HasKey("CollectionId", "FacetBoostId");

                    b.HasIndex("FacetBoostId");

                    b.ToTable("CollectionFacetBoost");
                });

            modelBuilder.Entity("Fame.Data.Models.CollectionOccasion", b =>
                {
                    b.Property<string>("CollectionId");

                    b.Property<string>("OccasionId");

                    b.HasKey("CollectionId", "OccasionId");

                    b.HasIndex("OccasionId");

                    b.ToTable("CollectionOccasion");
                });

            modelBuilder.Entity("Fame.Data.Models.CollectionProduct", b =>
                {
                    b.Property<string>("CollectionId");

                    b.Property<string>("ProductId");

                    b.HasKey("CollectionId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("CollectionProduct");
                });

            modelBuilder.Entity("Fame.Data.Models.CompatibleOption", b =>
                {
                    b.Property<int>("CompatibleOptionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<bool>("Indexed");

                    b.Property<bool>("IsolateInSummary");

                    b.Property<string>("ManufacturingSortOrderId");

                    b.Property<int?>("PreviewZoom");

                    b.Property<string>("RenderPositionId");

                    b.Property<int>("Sort");

                    b.Property<string>("Title");

                    b.HasKey("ComponentId");

                    b.HasIndex("ComponentTypeId");

                    b.HasIndex("ManufacturingSortOrderId");

                    b.HasIndex("RenderPositionId");

                    b.ToTable("Component");
                });

            modelBuilder.Entity("Fame.Data.Models.ComponentMeta", b =>
                {
                    b.Property<string>("ComponentId");

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("ComponentId", "Key");

                    b.ToTable("ComponentMeta");
                });

            modelBuilder.Entity("Fame.Data.Models.ComponentType", b =>
                {
                    b.Property<string>("ComponentTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AggregateOnIndex");

                    b.Property<int>("ComponentTypeCategory");

                    b.Property<bool>("IsProductCode");

                    b.Property<string>("ParentComponentTypeId");

                    b.Property<string>("SelectionTitle");

                    b.Property<decimal>("SortWeightDefault")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("SortWeightOther")
                        .HasColumnType("decimal(5,2)");

                    b.Property<string>("Title");

                    b.HasKey("ComponentTypeId");

                    b.HasIndex("ParentComponentTypeId");

                    b.ToTable("ComponentType");
                });

            modelBuilder.Entity("Fame.Data.Models.Curation", b =>
                {
                    b.Property<string>("PID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("OverlayText");

                    b.Property<string>("PrimarySilhouetteId");

                    b.Property<string>("ProductDocumentVersionId");

                    b.Property<string>("ProductId");

                    b.Property<string>("TaxonString");

                    b.HasKey("PID");

                    b.HasIndex("PrimarySilhouetteId");

                    b.HasIndex("ProductId");

                    b.ToTable("Curation");
                });

            modelBuilder.Entity("Fame.Data.Models.CurationComponent", b =>
                {
                    b.Property<string>("PID");

                    b.Property<string>("ComponentId");

                    b.HasKey("PID", "ComponentId");

                    b.HasIndex("ComponentId");

                    b.ToTable("CurationComponent");
                });

            modelBuilder.Entity("Fame.Data.Models.CurationMedia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Archived");

                    b.Property<string>("FitDescription");

                    b.Property<DateTime?>("LastModified");

                    b.Property<string>("PID");

                    b.Property<int>("PLPSortOrder");

                    b.Property<string>("SizeDescription");

                    b.Property<int>("SortOrder");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("PID");

                    b.ToTable("CurationMedia");
                });

            modelBuilder.Entity("Fame.Data.Models.CurationMediaVariant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CurationMediaId");

                    b.Property<string>("Ext");

                    b.Property<int>("Height");

                    b.Property<bool>("IsOriginal");

                    b.Property<int>("Quality");

                    b.Property<int>("Width");

                    b.HasKey("Id");

                    b.HasIndex("CurationMediaId");

                    b.ToTable("CurationMediaVariant");
                });

            modelBuilder.Entity("Fame.Data.Models.Facet", b =>
                {
                    b.Property<string>("FacetId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompatibilityRule");

                    b.Property<string>("Description");

                    b.Property<string>("FacetGroupId");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<string>("PreviewImage");

                    b.Property<string>("Subtitle");

                    b.Property<int>("TagPriority");

                    b.Property<string>("TaxonString");

                    b.Property<string>("Title");

                    b.HasKey("FacetId");

                    b.HasIndex("FacetGroupId");

                    b.ToTable("Facet");
                });

            modelBuilder.Entity("Fame.Data.Models.FacetBoost", b =>
                {
                    b.Property<int>("FacetBoostId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BoostRule");

                    b.Property<decimal>("BoostWeight");

                    b.HasKey("FacetBoostId");

                    b.ToTable("FacetBoost");
                });

            modelBuilder.Entity("Fame.Data.Models.FacetCategory", b =>
                {
                    b.Property<string>("FacetCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HideHeader");

                    b.Property<int>("Sort");

                    b.Property<string>("Title");

                    b.HasKey("FacetCategoryId");

                    b.ToTable("FacetCategory");
                });

            modelBuilder.Entity("Fame.Data.Models.FacetCategoryConfiguration", b =>
                {
                    b.Property<string>("FacetCategoryId");

                    b.Property<string>("FacetConfigurationId");

                    b.HasKey("FacetCategoryId", "FacetConfigurationId");

                    b.HasIndex("FacetConfigurationId");

                    b.ToTable("FacetCategoryConfiguration");
                });

            modelBuilder.Entity("Fame.Data.Models.FacetCategoryGroup", b =>
                {
                    b.Property<string>("FacetCategoryId");

                    b.Property<string>("FacetGroupId");

                    b.HasKey("FacetCategoryId", "FacetGroupId");

                    b.HasIndex("FacetGroupId");

                    b.ToTable("FacetCategoryGroup");
                });

            modelBuilder.Entity("Fame.Data.Models.FacetConfiguration", b =>
                {
                    b.Property<string>("FacetConfigurationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.HasKey("FacetConfigurationId");

                    b.ToTable("FacetConfiguration");
                });

            modelBuilder.Entity("Fame.Data.Models.FacetGroup", b =>
                {
                    b.Property<string>("FacetGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Collapsed");

                    b.Property<bool>("IsAggregatedFacet");

                    b.Property<bool>("IsCategoryFacet");

                    b.Property<bool>("Multiselect");

                    b.Property<string>("Name");

                    b.Property<bool>("PrimarySilhouette");

                    b.Property<int?>("ProductNameOrder");

                    b.Property<string>("Slug");

                    b.Property<int>("Sort");

                    b.Property<string>("Subtitle");

                    b.Property<string>("Title");

                    b.HasKey("FacetGroupId");

                    b.ToTable("FacetGroup");
                });

            modelBuilder.Entity("Fame.Data.Models.FacetMeta", b =>
                {
                    b.Property<string>("FacetId");

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("FacetId", "Key");

                    b.ToTable("FacetMeta");
                });

            modelBuilder.Entity("Fame.Data.Models.FeedMeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Ext");

                    b.Property<bool>("Zipped");

                    b.HasKey("Id");

                    b.ToTable("FeedMeta");
                });

            modelBuilder.Entity("Fame.Data.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Hidden");

                    b.Property<int>("Order");

                    b.Property<int>("ProductVersionId");

                    b.Property<string>("SelectionTitle");

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("GroupId");

                    b.HasIndex("ProductVersionId");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Fame.Data.Models.Incompatibility", b =>
                {
                    b.Property<int>("IncompatibilityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OptionId");

                    b.Property<int?>("ParentOptionId");

                    b.HasKey("IncompatibilityId");

                    b.HasIndex("OptionId");

                    b.HasIndex("ParentOptionId");

                    b.ToTable("Incompatibility");
                });

            modelBuilder.Entity("Fame.Data.Models.IncompatibleOption", b =>
                {
                    b.Property<int>("IncompatibilityId");

                    b.Property<int>("OptionId");

                    b.HasKey("IncompatibilityId", "OptionId");

                    b.HasIndex("OptionId");

                    b.ToTable("IncompatibleOption");
                });

            modelBuilder.Entity("Fame.Data.Models.ManufacturingSortOrder", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Order");

                    b.HasKey("Id");

                    b.ToTable("ManufacturingSortOrder");
                });

            modelBuilder.Entity("Fame.Data.Models.Occasion", b =>
                {
                    b.Property<string>("OccasionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ComponentCompatibilityRule");

                    b.Property<string>("FacetCompatibilityRule");

                    b.Property<string>("OccasionName");

                    b.HasKey("OccasionId");

                    b.ToTable("Occasion");
                });

            modelBuilder.Entity("Fame.Data.Models.Option", b =>
                {
                    b.Property<int>("OptionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<int>("PriceInMinorUnits");

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

                    b.Property<bool>("DisableLayering");

                    b.Property<string>("DropBoxAssetFolder");

                    b.Property<string>("DropName");

                    b.Property<bool>("Index");

                    b.Property<int>("PreviewType");

                    b.Property<int>("ProductType");

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Factory");

                    b.Property<string>("ProductId");

                    b.Property<int>("VersionState");

                    b.HasKey("ProductVersionId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductVersion");
                });

            modelBuilder.Entity("Fame.Data.Models.ProductVersionPrice", b =>
                {
                    b.Property<int>("ProductVersionId");

                    b.Property<string>("LocalisationCode");

                    b.Property<int>("PriceInMinorUnits");

                    b.HasKey("ProductVersionId", "LocalisationCode");

                    b.ToTable("ProductVersionPrice");
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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AggregateTitle");

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

            modelBuilder.Entity("Fame.Data.Models.Workflow", b =>
                {
                    b.Property<int>("WorkflowStep");

                    b.Property<DateTimeOffset?>("TriggeredDateTime");

                    b.HasKey("WorkflowStep");

                    b.ToTable("Workflow");
                });

            modelBuilder.Entity("Fame.Data.Models.CollectionFacet", b =>
                {
                    b.HasOne("Fame.Data.Models.Collection", "Collection")
                        .WithMany("Facets")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Facet", "Facet")
                        .WithMany("Collections")
                        .HasForeignKey("FacetId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.CollectionFacetBoost", b =>
                {
                    b.HasOne("Fame.Data.Models.Collection", "Collection")
                        .WithMany("FacetBoosts")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.FacetBoost", "FacetBoost")
                        .WithMany("Collections")
                        .HasForeignKey("FacetBoostId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.CollectionOccasion", b =>
                {
                    b.HasOne("Fame.Data.Models.Collection", "Collection")
                        .WithMany("Occasions")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Occasion", "Occasion")
                        .WithMany("Collections")
                        .HasForeignKey("OccasionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.CollectionProduct", b =>
                {
                    b.HasOne("Fame.Data.Models.Collection", "Collection")
                        .WithMany("Products")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Product", "Product")
                        .WithMany("Collections")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
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
                        .WithMany("Components")
                        .HasForeignKey("ComponentTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.ManufacturingSortOrder", "ManufacturingSortOrder")
                        .WithMany("Components")
                        .HasForeignKey("ManufacturingSortOrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.RenderPosition", "RenderPosition")
                        .WithMany("Components")
                        .HasForeignKey("RenderPositionId");
                });

            modelBuilder.Entity("Fame.Data.Models.ComponentMeta", b =>
                {
                    b.HasOne("Fame.Data.Models.Component", "Component")
                        .WithMany("ComponentMeta")
                        .HasForeignKey("ComponentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fame.Data.Models.ComponentType", b =>
                {
                    b.HasOne("Fame.Data.Models.ComponentType", "ParentComponentType")
                        .WithMany("ChildComponentTypes")
                        .HasForeignKey("ParentComponentTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.Curation", b =>
                {
                    b.HasOne("Fame.Data.Models.Facet", "Facet")
                        .WithMany("Curations")
                        .HasForeignKey("PrimarySilhouetteId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Product", "Product")
                        .WithMany("Curations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.CurationComponent", b =>
                {
                    b.HasOne("Fame.Data.Models.Component", "Component")
                        .WithMany("CurationComponents")
                        .HasForeignKey("ComponentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Curation", "Curation")
                        .WithMany("CurationComponents")
                        .HasForeignKey("PID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.CurationMedia", b =>
                {
                    b.HasOne("Fame.Data.Models.Curation", "Curation")
                        .WithMany("Media")
                        .HasForeignKey("PID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.CurationMediaVariant", b =>
                {
                    b.HasOne("Fame.Data.Models.CurationMedia", "CurationMedia")
                        .WithMany("CurationMediaVariants")
                        .HasForeignKey("CurationMediaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fame.Data.Models.Facet", b =>
                {
                    b.HasOne("Fame.Data.Models.FacetGroup", "FacetGroup")
                        .WithMany("Facets")
                        .HasForeignKey("FacetGroupId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.FacetCategoryConfiguration", b =>
                {
                    b.HasOne("Fame.Data.Models.FacetCategory", "FacetCategory")
                        .WithMany("FacetCategoryConfigurations")
                        .HasForeignKey("FacetCategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.FacetConfiguration", "FacetConfiguration")
                        .WithMany("FacetCategoryConfigurations")
                        .HasForeignKey("FacetConfigurationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.FacetCategoryGroup", b =>
                {
                    b.HasOne("Fame.Data.Models.FacetCategory", "FacetCategory")
                        .WithMany("FacetCategoryGroups")
                        .HasForeignKey("FacetCategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.FacetGroup", "FacetGroup")
                        .WithMany("FacetCategoryGroups")
                        .HasForeignKey("FacetGroupId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.FacetMeta", b =>
                {
                    b.HasOne("Fame.Data.Models.Facet", "Facet")
                        .WithMany("FacetMeta")
                        .HasForeignKey("FacetId")
                        .OnDelete(DeleteBehavior.Cascade);
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
                    b.HasOne("Fame.Data.Models.Option", "Option")
                        .WithMany("Incompatibilities")
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Option", "ParentOption")
                        .WithMany("IncompatibleParentOptions")
                        .HasForeignKey("ParentOptionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fame.Data.Models.IncompatibleOption", b =>
                {
                    b.HasOne("Fame.Data.Models.Incompatibility", "Incompatibility")
                        .WithMany("IncompatibleOptions")
                        .HasForeignKey("IncompatibilityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fame.Data.Models.Option", "Option")
                        .WithMany("IncompatibleOptions")
                        .HasForeignKey("OptionId")
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

            modelBuilder.Entity("Fame.Data.Models.ProductVersionPrice", b =>
                {
                    b.HasOne("Fame.Data.Models.ProductVersion", "ProductVersion")
                        .WithMany("Prices")
                        .HasForeignKey("ProductVersionId")
                        .OnDelete(DeleteBehavior.Restrict);
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