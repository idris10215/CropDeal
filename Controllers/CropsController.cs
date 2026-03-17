using Microsoft.AspNetCore.Mvc;
using CropDeal.Models;
using CropDeal.Services;
using Microsoft.AspNetCore.Authorization;

namespace CropDeal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CropsController : ControllerBase
    {
        private readonly ICropService _cropService;

        public CropsController(ICropService cropService)
        {
            _cropService = cropService;
        }

        [HttpGet("catalog")]
        public async Task<IActionResult> GetCrops()
        {
            var crops = await _cropService.GetCatalogAsync();
            return Ok(crops);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCrop(CropCreateDto dto)
        {
            try 
            {
                await _cropService.AddCropAsync(dto);
                return Ok(new { message = "Crop added to catalog successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}