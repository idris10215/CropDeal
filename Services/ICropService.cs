using CropDeal.Models;

namespace CropDeal.Services
{
    public interface ICropService
    {
        Task<IEnumerable<CropReadDto>> GetCatalogAsync();
        
        Task AddCropAsync(CropCreateDto dto);
    }
}