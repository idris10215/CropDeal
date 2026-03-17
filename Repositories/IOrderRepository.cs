using CropDeal.Models;

namespace CropDeal.Repositories;

public interface IOrderRepository 
{
    Task AddOrderAsync(Order order);
    Task SaveChangesAsync();
    
    // --- New Methods for the Workflow ---
    Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(int buyerId);
    Task<IEnumerable<Order>> GetOrdersByFarmerIdAsync(int farmerId);
    Task<Order?> GetOrderByIdAsync(int id);
}