using Core.Entities.Concrete;
using Entities.Common;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=ECommerceArchAPIDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLanguage> CategoryLanguages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLanguage> ProductLanguages { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<SpecificationLanguage> SpecificationLanguages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Entities.Common.File> Files { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdatedDate = DateTime.Now;
                        break;
                    default:
                        data.Entity.CreatedDate = DateTime.Now;
                        break;
                }
            }
                return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdatedDate = DateTime.Now;
                        break;
                    default:
                        data.Entity.CreatedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductSize>()
            .HasKey(ps => new { ps.ProductId, ps.SizeId });

        modelBuilder.Entity<ProductSize>()
            .HasOne(ps => ps.Product)
            .WithMany(p => p.ProductSizes)
            .HasForeignKey(ps => ps.ProductId);

        modelBuilder.Entity<ProductSize>()
            .HasOne(ps => ps.Size)
            .WithMany(s => s.ProductSizes)
            .HasForeignKey(ps => ps.SizeId);

            modelBuilder.Entity<ProductSubCategory>()
           .HasKey(psc => new { psc.ProductId, psc.SubCategoryId });

            modelBuilder.Entity<ProductSubCategory>()
                .HasOne(psc => psc.Product)
                .WithMany(p => p.ProductSubCategories)
                .HasForeignKey(psc => psc.ProductId);

            modelBuilder.Entity<ProductSubCategory>()
                .HasOne(psc => psc.SubCategory)
                .WithMany(sc => sc.ProductSubCategories)
                .HasForeignKey(psc => psc.SubCategoryId);

            modelBuilder.Entity<IdentityUserLogin<string>>()
        .HasKey(x => new { x.LoginProvider, x.ProviderKey });

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
        }


    }
}
