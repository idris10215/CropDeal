using CropDeal.Models;

namespace CropDeal.Services;

public interface IOrderService 
{
    Task<string> PlaceOrderAsync(OrderCreateDto dto);
    Task<IEnumerable<Order>> GetBuyerOrdersAsync(int buyerId);
    Task<IEnumerable<Order>> GetFarmerOrdersAsync(int farmerId);
    Task<string> ApproveOrderAsync(int orderId);
}