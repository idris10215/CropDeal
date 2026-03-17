using CropDeal.Models;

namespace CropDeal.Services
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}