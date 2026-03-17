using Microsoft.AspNetCore.Mvc;
using CropDeal.Models;
using CropDeal.Services;

namespace CropDeal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(OrderCreateDto dto)
        {
            var result = await _orderService.PlaceOrderAsync(dto);
            return Ok(new { message = result });
        }
    }
}