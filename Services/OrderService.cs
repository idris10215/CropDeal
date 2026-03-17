using CropDeal.Models;
using CropDeal.Repositories;

namespace CropDeal.Services;

public interface IOrderService {
    Task<string> PlaceOrderAsync(OrderCreateDto dto);
}

public class OrderService : IOrderService {
    private readonly IOrderRepository _orderRepo;
    public OrderService(IOrderRepository orderRepo) { _orderRepo = orderRepo; }

    public async Task<string> PlaceOrderAsync(OrderCreateDto dto)
    {
        var order = new Order
        {
            CropId = dto.CropId,
            BuyerId = dto.BuyerId,
            TotalAmount = dto.TotalAmount,
            OrderDate = DateTime.Now,
            Status = "Pending"
        };

        await _orderRepo.AddOrderAsync(order);
        await _orderRepo.SaveChangesAsync();
        return "Order placed successfully!";
    }
}