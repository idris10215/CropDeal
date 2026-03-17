using CropDeal.Models;

namespace CropDeal.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}