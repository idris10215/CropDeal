using Microsoft.EntityFrameworkCore;
using CropDeal.Models;

namespace CropDeal.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Crop> Crops { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Fix Decimal Precision
        modelBuilder.Entity<Crop>().Property(c => c.Price).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

        // 2. Seller -> Crop Relationship
        modelBuilder.Entity<Crop>()
            .HasOne(c => c.Seller)
            .WithMany(u => u.Crops)
            .HasForeignKey(c => c.SellerId)
            .OnDelete(DeleteBehavior.Cascade); // Keeping cascade here is fine

        // 3. THE FIX: Buyer -> Order Relationship
        // We change this to 'Restrict' or 'NoAction' to stop the circular path error
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Buyer)
            .WithMany() 
            .HasForeignKey(o => o.BuyerId)
            .OnDelete(DeleteBehavior.Restrict); 

        // 4. Crop -> Order Relationship
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Crop)
            .WithMany()
            .HasForeignKey(o => o.CropId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}