using CropDeal.Models;
using CropDeal.Repositories;

namespace CropDeal.Services
{
    public class CropService : ICropService
    {
        private readonly ICropRepository _cropRepo;

        public CropService(ICropRepository cropRepo)
        {
            _cropRepo = cropRepo;
        }

        public async Task<IEnumerable<CropReadDto>> GetCatalogAsync()
        {
            var crops = await _cropRepo.GetAllCropsAsync();
            
            // Converting Entity list to DTO list
            return crops.Select(c => new CropReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Category = c.Category,
                Price = c.Price,
                Quantity = c.Quantity,
                SellerName = c.Seller?.FullName ?? "Unknown"
            }).ToList(); // .ToList() ensures the types match exactly
        }

        public async Task AddCropAsync(CropCreateDto dto)
        {
            var crop = new Crop
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Quantity = dto.Quantity,
                SellerId = dto.SellerId
            };

            await _cropRepo.AddCropAsync(crop);
            await _cropRepo.SaveChangesAsync();
        }
    }
}