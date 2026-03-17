using CropDeal.Models;
using CropDeal.Data;
using Microsoft.EntityFrameworkCore;

namespace CropDeal.Repositories
{
    public class CropRepository : ICropRepository
    {
        private readonly AppDbContext _context;
        public CropRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<Crop>> GetAllCropsAsync() 
            => await _context.Crops.Include(c => c.Seller).ToListAsync();

        public async Task AddCropAsync(Crop crop) 
            => await _context.Crops.AddAsync(crop);

        public async Task SaveChangesAsync() 
            => await _context.SaveChangesAsync();
    }
}