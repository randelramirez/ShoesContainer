using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalogApi.Domain;

namespace ProductCatalogApi.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatalogBrand>(ConfigureCatalogBrand);
            modelBuilder.Entity<CatalogType>(ConfigureCatalogType);
            modelBuilder.Entity<CatalogItem>(ConfigureCatalogItem);
        }

        private void ConfigureCatalogBrand(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand");
            builder.Property(c => c.Id)
                .UseHiLo("catalog_brand_hilo") //.ForSqlServerUseSequenceHiLo("catalog_hilo");
                .IsRequired();
            builder.Property(c => c.Brand)
                .IsRequired()
                .HasMaxLength(100);
        }

        private void ConfigureCatalogType(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogType");
            builder.Property(c => c.Id)
                .UseHiLo("catalog_type_hilo") //.ForSqlServerUseSequenceHiLo("catalog_hilo");
                .IsRequired();
            builder.Property(c => c.Type)
                .IsRequired()
                .HasMaxLength(100);
        }

        private void ConfigureCatalogItem(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog");
            builder.Property(c => c.Id)
                .UseHiLo("catalog_hilo") //.ForSqlServerUseSequenceHiLo("catalog_hilo");
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.Price)
                .IsRequired(true);
            builder.Property(c => c.PictureUrl)
                .IsRequired(false);

            builder.HasOne(c => c.CatalogBrand)
                .WithMany()
                .HasForeignKey(c => c.CatalogBrandId);

            builder.HasOne(c => c.CatalogType)
                .WithMany()
                .HasForeignKey(c => c.CatalogTypedId);
        }

        public DbSet<CatalogBrand> CatalogBrands { get; set; }

        public DbSet<CatalogBrand> CatalogItems { get; set; }

        public DbSet<CatalogBrand> CatalogTypes { get; set; }
    }
}