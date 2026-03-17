using CropDeal.Models;

namespace CropDeal.Services
{
    public interface ICropService
    {
        Task<IEnumerable<CropReadDto>> GetCatalogAsync();
        
        Task AddCropAsync(CropCreateDto dto);

        Task<bool> UpdateCropAsync(int id, CropCreateDto dto);
        Task<bool> DeleteCropAsync(int id);
    }
}