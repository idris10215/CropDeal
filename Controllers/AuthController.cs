using Microsoft.AspNetCore.Mvc;
using CropDeal.Data;
using CropDeal.Models;

namespace CropDeal.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        _context.users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }

}