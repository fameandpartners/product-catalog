using Microsoft.EntityFrameworkCore;
using Fame.Data.Models;

namespace Fame.Data
{
    public class FameContext : DbContext
    {
        public FameContext(DbContextOptions options)
        : base(options)
        { }
        
        public DbSet<Component> Component { get; set; }
        public DbSet<ComponentType> ComponentType { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Incompatibility> Incompatibility { get; set; }
        public DbSet<Option> Option { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductVersion> ProductVersion { get; set; }
        public DbSet<ProductRenderComponent> ProductRenderComponent { get; set; }
        public DbSet<OptionRenderComponent> OptionRenderComponent { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<SectionGroup> SectionGroup { get; set; }
        public DbSet<OptionPrice> OptionPrice { get; set; }
        public DbSet<CompatibleOption> CompatibleOption { get; set; }
        public DbSet<RenderPosition> RenderPosition { get; set; }
        public DbSet<Facet> Facet { get; set; }
        public DbSet<FacetGroup> FacetGroup { get; set; }
        public DbSet<FacetCategory> FacetCategory { get; set; }
        public DbSet<FacetConfiguration> FacetConfiguration { get; set; }
        public DbSet<FacetCategoryGroup> FacetCategoryGroup { get; set; }
        public DbSet<FacetCategoryConfiguration> FacetCategoryConfiguration { get; set; }
        public DbSet<ManufacturingSortOrder> ManufacturingSortOrder { get; set; }
        public DbSet<ProductVersionPrice> ProductVersionPrice { get; set; }
        public DbSet<FacetMeta> FacetMeta { get; set; }
        public DbSet<FacetBoost> FacetBoost { get; set; }
        public DbSet<Curation> Curation { get; set; }
        public DbSet<CurationMedia> CurationMedia { get; set; }
        public DbSet<CurationComponent> CurationComponent { get; set; }
        public DbSet<Occasion> Occasion { get; set; }
        public DbSet<FeedMeta> FeedMeta { get; set; }
        public DbSet<Collection> Collection { get; set; }
        public DbSet<CollectionProduct> CollectionProduct { get; set; }
        public DbSet<CollectionFacet> CollectionFacet { get; set; }
        public DbSet<CollectionOccasion> CollectionOccasion { get; set; }
        public DbSet<Workflow> Workflow { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collection>()
                .HasKey(c => new { c.CollectionId });

            modelBuilder.Entity<Workflow>()
                .HasKey(c => new { c.WorkflowStep });

            modelBuilder.Entity<CollectionProduct>()
                .HasKey(c => new { c.CollectionId, c.ProductId });

            modelBuilder.Entity<CollectionFacet>()
                .HasKey(c => new { c.CollectionId, c.FacetId });

            modelBuilder.Entity<CollectionFacetBoost>()
                .HasKey(c => new { c.CollectionId, c.FacetBoostId });

            modelBuilder.Entity<CollectionOccasion>()
                .HasKey(c => new { c.CollectionId, c.OccasionId });

            modelBuilder.Entity<CollectionProduct>()
                .HasOne(s => s.Collection)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.CollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CollectionProduct>()
                .HasOne(s => s.Product)
                .WithMany(c => c.Collections)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CollectionFacet>()
                .HasOne(s => s.Collection)
                .WithMany(c => c.Facets)
                .HasForeignKey(c => c.CollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CollectionFacet>()
                .HasOne(s => s.Facet)
                .WithMany(c => c.Collections)
                .HasForeignKey(c => c.FacetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CollectionOccasion>()
                .HasOne(s => s.Collection)
                .WithMany(c => c.Occasions)
                .HasForeignKey(c => c.CollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CollectionOccasion>()
                .HasOne(s => s.Occasion)
                .WithMany(c => c.Collections)
                .HasForeignKey(c => c.OccasionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CollectionFacetBoost>()
                .HasOne(s => s.Collection)
                .WithMany(c => c.FacetBoosts)
                .HasForeignKey(c => c.CollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CollectionFacetBoost>()
                .HasOne(s => s.FacetBoost)
                .WithMany(c => c.Collections)
                .HasForeignKey(c => c.FacetBoostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FeedMeta>()
                .HasKey(c => new { c.Id });

            modelBuilder.Entity<Occasion>()
                .HasKey(c => new { c.OccasionId });

            modelBuilder.Entity<Curation>()
                .HasKey(c => new { c.PID });

            modelBuilder.Entity<CurationComponent>()
                .HasKey(c => new { c.PID, c.ComponentId });
            
            modelBuilder.Entity<CurationComponent>()
                .HasOne(s => s.Curation)
                .WithMany(c => c.CurationComponents)
                .HasForeignKey(c => c.PID)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Curation>()
                .HasOne(s => s.Facet)
                .WithMany(c => c.Curations)
                .HasForeignKey(c => c.PrimarySilhouetteId)
                .OnDelete(DeleteBehavior.Restrict)
				.IsRequired(false);

            modelBuilder.Entity<CurationComponent>()
                .HasOne(s => s.Component)
                .WithMany(c => c.CurationComponents)
                .HasForeignKey(c => c.ComponentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Curation>()
                .HasOne(s => s.Product)
                .WithMany(c => c.Curations)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CurationMedia>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Curation>()
                .HasMany(s => s.Media)
                .WithOne(c => c.Curation)
                .HasForeignKey(c => c.PID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ManufacturingSortOrder>()
                .HasKey(c => new {c.Id});

            modelBuilder.Entity<FacetBoost>()
                .HasKey(c => new {c.FacetBoostId});

            modelBuilder.Entity<ManufacturingSortOrder>()
                .HasMany(s => s.Components)
                .WithOne(c => c.ManufacturingSortOrder)
                .HasForeignKey(c => c.ManufacturingSortOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FacetGroup>()
                .HasKey(c => new { c.FacetGroupId });

            modelBuilder.Entity<FacetCategory>()
                .HasKey(c => new { c.FacetCategoryId });

            modelBuilder.Entity<FacetConfiguration>()
                .HasKey(c => new { c.FacetConfigurationId });

            modelBuilder.Entity<FacetCategoryGroup>()
                .HasKey(c => new { c.FacetCategoryId, c.FacetGroupId });

            modelBuilder.Entity<IncompatibleOption>()
                .HasKey(c => new { c.IncompatibilityId, c.OptionId });
            
            modelBuilder.Entity<IncompatibleOption>()
                .HasOne(c => c.Incompatibility)
                .WithMany(c => c.IncompatibleOptions)
                .HasForeignKey(c => c.IncompatibilityId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            modelBuilder.Entity<IncompatibleOption>()
                .HasOne(c => c.Option)
                .WithMany(c => c.IncompatibleOptions)
                .HasForeignKey(c => c.OptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            modelBuilder.Entity<FacetCategoryGroup>()
                .HasOne(c => c.FacetGroup)
                .WithMany(c => c.FacetCategoryGroups)
                .HasForeignKey(c => c.FacetGroupId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            modelBuilder.Entity<FacetCategoryGroup>()
                .HasOne(c => c.FacetCategory)
                .WithMany(c => c.FacetCategoryGroups)
                .HasForeignKey(c => c.FacetCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<FacetCategoryConfiguration>()
                .HasKey(c => new { c.FacetCategoryId, c.FacetConfigurationId });
            
            modelBuilder.Entity<FacetCategoryConfiguration>()
                .HasOne(c => c.FacetConfiguration)
                .WithMany(c => c.FacetCategoryConfigurations)
                .HasForeignKey(c => c.FacetConfigurationId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            modelBuilder.Entity<FacetCategoryConfiguration>()
                .HasOne(c => c.FacetCategory)
                .WithMany(c => c.FacetCategoryConfigurations)
                .HasForeignKey(c => c.FacetCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            modelBuilder.Entity<ComponentMeta>()
                .HasKey(c => new { c.ComponentId, c.Key });
            
            modelBuilder.Entity<FacetMeta>()
                .HasKey(c => new { c.FacetId, c.Key });

            modelBuilder.Entity<ComponentType>()
                .Property(b => b.SortWeightDefault)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            modelBuilder.Entity<ComponentType>()
                .Property(b => b.SortWeightOther)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            modelBuilder.Entity<OptionPrice>()
                .Property(b => b.PriceInMinorUnits)
                .IsRequired();

            modelBuilder.Entity<ProductVersionPrice>()
                .Property(b => b.PriceInMinorUnits)
                .IsRequired();

            modelBuilder.Entity<Component>()
                .HasOne(s => s.ComponentType)
                .WithMany(c => c.Components)
                .HasForeignKey(c => c.ComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Component>()
                .Property(b => b.IsolateInSummary)
                .IsRequired();

            modelBuilder.Entity<Facet>()
                .HasOne(s => s.FacetGroup)
                .WithMany(c => c.Facets)
                .HasForeignKey(c => c.FacetGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Option>()
                .HasKey(c => new { c.OptionId });
            
            modelBuilder.Entity<Option>()
                .HasIndex(c => new { c.ComponentId, c.ProductVersionId })
                .IsUnique();

            modelBuilder.Entity<CompatibleOption>()
                .HasKey(c => new { c.CompatibleOptionId });

            modelBuilder.Entity<CompatibleOption>()
                .HasIndex(c => new { c.SectionId, c.OptionId, c.ParentOptionId })
                .IsUnique();

            modelBuilder.Entity<RenderPosition>()
                .HasKey(c => new { c.RenderPositionId });

            modelBuilder.Entity<ComponentType>()
                .HasKey(c => new { c.ComponentTypeId });

            modelBuilder.Entity<OptionPrice>()
                .HasOne(c => c.Option)
                .WithMany(c => c.Prices)
                .HasForeignKey(c => c.OptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductVersionPrice>()
                .HasOne(c => c.ProductVersion)
                .WithMany(c => c.Prices)
                .HasForeignKey(c => c.ProductVersionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incompatibility>()
                .HasKey(c => new { c.IncompatibilityId });

            modelBuilder.Entity<Incompatibility>()
                .HasOne(c => c.Option)
                .WithMany(c => c.Incompatibilities)
                .HasForeignKey(c => c.OptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Incompatibility>()
                .HasOne(c => c.ParentOption)
                .WithMany(c => c.IncompatibleParentOptions)
                .HasForeignKey(c => c.ParentOptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<ProductRenderComponent>()
                .HasKey(c => new {c.ComponentTypeId, c.ProductVersionId});

            modelBuilder.Entity<ProductRenderComponent>()
                .HasOne(c => c.ComponentType)
                .WithMany(c => c.ProductRenderComponents)
                .HasForeignKey(c => c.ComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductRenderComponent>()
                .HasOne(c => c.ProductVersion)
                .WithMany(c => c.ProductRenderComponents)
                .HasForeignKey(c => c.ProductVersionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompatibleOption>()
                .HasOne(c => c.ParentOption)
                .WithMany()
                .HasForeignKey(c => c.ParentOptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<CompatibleOption>()
                .HasOne(c => c.Option)
                .WithMany(o => o.CompatibleOptions)
                .HasForeignKey(c => c.OptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompatibleOption>()
                .HasOne(c => c.Section)
                .WithMany(c => c.CompatibleOptions)
                .HasForeignKey(c => c.SectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OptionRenderComponent>()
                .HasKey(c => new {c.ComponentTypeId, c.OptionId});

            modelBuilder.Entity<OptionRenderComponent>()
                .HasOne(c => c.ComponentType)
                .WithMany(c => c.OptionRenderComponents)
                .HasForeignKey(c => c.ComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OptionRenderComponent>()
                .HasOne(c => c.Option)
                .WithMany(c => c.OptionRenderComponents)
                .HasForeignKey(c => c.OptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComponentType>()
                .HasOne(c => c.ParentComponentType)
                .WithMany(c => c.ChildComponentTypes)
                .HasForeignKey(c => c.ParentComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Section>()
                .HasOne(c => c.ComponentType)
                .WithMany(c => c.Sections)
                .HasForeignKey(c => c.ComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OptionPrice>()
                .HasKey(c => new {c.OptionId, c.LocalisationCode});

            modelBuilder.Entity<ProductVersionPrice>()
                .HasKey(c => new {c.ProductVersionId, c.LocalisationCode});
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
