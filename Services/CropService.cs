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
            return crops.Select(c => new CropReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Category = c.Category,
                Price = c.Price,
                Quantity = c.Quantity,
                SellerName = c.Seller?.FullName ?? "Unknown"
            }).ToList();
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

        public async Task<bool> UpdateCropAsync(int id, CropCreateDto dto)
        {
            var crop = await _cropRepo.GetCropByIdAsync(id);
            if (crop == null) return false;

            crop.Name = dto.Name;
            crop.Category = dto.Category;
            crop.Price = dto.Price;
            crop.Quantity = dto.Quantity;

            await _cropRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCropAsync(int id)
        {
            var crop = await _cropRepo.GetCropByIdAsync(id);
            if (crop == null) return false;

            _cropRepo.DeleteCrop(crop);
            await _cropRepo.SaveChangesAsync();
            return true;
        }
    }
}