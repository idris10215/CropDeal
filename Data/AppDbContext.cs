using Microsoft.EntityFrameworkCore;
using CropDeal.Models;
using Microsoft.Identity.Client;

namespace CropDeal.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
        public DbSet<User> users {get; set;}
    
}