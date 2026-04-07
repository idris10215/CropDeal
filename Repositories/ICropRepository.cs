using CropDeal.Models;

namespace CropDeal.Repositories
{
    public interface ICropRepository
    {
        Task<IEnumerable<Crop>> GetAllCropsAsync(); 
        Task AddCropAsync(Crop crop);
        Task SaveChangesAsync();
        Task<Crop?> GetCropByIdAsync(int id);
        void DeleteCrop(Crop crop);
    }
}