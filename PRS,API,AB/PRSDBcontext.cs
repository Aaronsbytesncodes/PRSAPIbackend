using Microsoft.EntityFrameworkCore;
using PRSBackendAB.models;
namespace PRSBackendAB


{ 
    public class PrsDbContext(DbContextOptions<PrsDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<LineItem> LineItems { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        //    modelBuilder.Entity<LineItem>().HasIndex(li => new { li.RequestId, li.ProductId }).IsUnique();
        //    modelBuilder.Entity<Product>().HasIndex(p => new { p.VendorId, p.PartNumber }).IsUnique();
        //    modelBuilder.Entity<Vendor>().HasIndex(v => v.Code).IsUnique();
        //}
    }
}
