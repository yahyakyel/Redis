using Microsoft.EntityFrameworkCore;

namespace Redis.Business.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData
            (
                new Product() { Id = Guid.NewGuid(), Name = "Kalem 1" },
                new Product() { Id = Guid.NewGuid(), Name = "Kalem 2" },
                new Product() { Id = Guid.NewGuid(), Name = "Kalem 3" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
