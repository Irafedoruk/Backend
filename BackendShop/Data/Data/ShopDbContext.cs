using BackendShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.Data.Data
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImageEntity> ProductImageEntity { get; set; }
        public DbSet<ProductDescImageEntity> ProductDescImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Product>()
            //    .Property(p => p.Price)
            //    .HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<Product>().ToTable("Products");
        }
    }
}
