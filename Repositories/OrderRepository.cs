using CropDeal.Models;
using CropDeal.Data;
using Microsoft.EntityFrameworkCore;

namespace CropDeal.Repositories;

public class OrderRepository : IOrderRepository 
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context) 
    { 
        _context = context; 
    }

    public async Task AddOrderAsync(Order order) => await _context.Orders.AddAsync(order);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();


    public async Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(int buyerId)
    {
        return await _context.Orders
            .Include(o => o.Crop) 
            .Where(o => o.BuyerId == buyerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByFarmerIdAsync(int farmerId)
    {
        return await _context.Orders
            .Include(o => o.Crop)
            .Include(o => o.Buyer) 
            .Where(o => o.Crop.SellerId == farmerId)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Crop) 
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}