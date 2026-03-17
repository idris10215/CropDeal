using CropDeal.Models;
using CropDeal.Repositories;

namespace CropDeal.Services;

public class OrderService : IOrderService 
{
    private readonly IOrderRepository _orderRepo;
    private readonly ICropRepository _cropRepo;

    public OrderService(IOrderRepository orderRepo, ICropRepository cropRepo) 
    { 
        _orderRepo = orderRepo; 
        _cropRepo = cropRepo;
    }

    public async Task<string> PlaceOrderAsync(OrderCreateDto dto) 
    {
        var order = new Order 
        {
            CropId = dto.CropId,
            BuyerId = dto.BuyerId,
            Quantity = dto.Quantity,
            TotalAmount = dto.TotalAmount,
            OrderDate = DateTime.Now,
            Status = "Pending" // Initial status
        };
        await _orderRepo.AddOrderAsync(order);
        await _orderRepo.SaveChangesAsync();
        return "Order placed! Awaiting farmer approval.";
    }

    public async Task<IEnumerable<Order>> GetBuyerOrdersAsync(int buyerId) 
        => await _orderRepo.GetOrdersByBuyerIdAsync(buyerId);

    public async Task<IEnumerable<Order>> GetFarmerOrdersAsync(int farmerId) 
        => await _orderRepo.GetOrdersByFarmerIdAsync(farmerId);

    public async Task<string> ApproveOrderAsync(int orderId) 
    {
        var order = await _orderRepo.GetOrderByIdAsync(orderId);
        if (order == null) return "Order not found.";
        if (order.Status == "Approved") return "Order is already approved.";

        var crop = await _cropRepo.GetCropByIdAsync(order.CropId);
        if (crop == null) return "Crop no longer exists.";

        if (crop.Quantity < order.Quantity) return "Insufficient stock to approve this order.";

        // Deduct stock and update status
        crop.Quantity -= order.Quantity;
        order.Status = "Approved";

        await _orderRepo.SaveChangesAsync();
        await _cropRepo.SaveChangesAsync();
        
        return "Order approved and stock updated successfully!";
    }
}