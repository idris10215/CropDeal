using CropDeal.Models;
using CropDeal.Repositories; // Linking to your Repo folder
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CropDeal.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<string> RegisterUserAsync(RegisterDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                isActive = true
            };
            
            await _userRepo.AddUserAsync(user);
            await _userRepo.SaveChangesAsync();
            return "User registered successfully!";
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepo.GetUserByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return null; 
            }

            return CreateToken(user);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}