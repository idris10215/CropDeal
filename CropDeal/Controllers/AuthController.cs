using Microsoft.AspNetCore.Mvc;
using CropDeal.Data;
using CropDeal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CropDeal.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

        _context.users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully!" });
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDto request)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if(user == null)
        {
            return BadRequest("User not found");
        }

        if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Wrong password.");
        }

        string token = CreateToken(user);

        return Ok(token);

    }




    
    private string CreateToken(User user)
    {
        // 1. Define the "Claims" (What's written on the ID badge)
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // 2. Get the Secret Key from appsettings.json
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("Jwt:Key").Value!));

        // 3. Sign the badge with your secret key
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        // 4. Create the Token object
        var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

        // 5. Convert to string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}