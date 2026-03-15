using Microsoft.EntityFrameworkCore;
using CropService.Models;

namespace CropService.Data;

public class CropDbContext : DbContext
{
    public CropDbContext(DbContextOptions<CropDbContext> options) : base(options) { }

    public DbSet<Crop> Crops { get; set; }
}