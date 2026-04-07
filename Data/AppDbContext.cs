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
        modelBuilder.Entity<Crop>().Property(c => c.Price).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Crop>()
            .HasOne(c => c.Seller)
            .WithMany(u => u.Crops)
            .HasForeignKey(c => c.SellerId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Buyer)
            .WithMany() 
            .HasForeignKey(o => o.BuyerId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Crop)
            .WithMany()
            .HasForeignKey(o => o.CropId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}