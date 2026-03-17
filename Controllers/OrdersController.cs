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

        [HttpGet("buyer/{buyerId}")]
        public async Task<IActionResult> GetBuyerOrders(int buyerId) 
            => Ok(await _orderService.GetBuyerOrdersAsync(buyerId));

        [HttpGet("farmer/{farmerId}")]
        public async Task<IActionResult> GetFarmerOrders(int farmerId) 
            => Ok(await _orderService.GetFarmerOrdersAsync(farmerId));

        [HttpPut("approve/{orderId}")]
        public async Task<IActionResult> ApproveOrder(int orderId) 
            => Ok(await _orderService.ApproveOrderAsync(orderId));
            }
}