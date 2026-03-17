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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCrop(int id, CropCreateDto cropDto)
        {
            var success = await _cropService.UpdateCropAsync(id, cropDto);
            if (!success) return NotFound("Crop not found.");
            
            return Ok(new { message = "Crop updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrop(int id)
        {
            var success = await _cropService.DeleteCropAsync(id);
            if (!success) return NotFound("Crop not found.");

            return Ok(new { message = "Crop deleted successfully!" });
        }
    }
}