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

    // 1. Get all orders placed by a specific Buyer
    public async Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(int buyerId)
    {
        return await _context.Orders
            .Include(o => o.Crop) // So the buyer can see the Crop Name
            .Where(o => o.BuyerId == buyerId)
            .ToListAsync();
    }

    // 2. Get all orders for crops belonging to a specific Farmer
    public async Task<IEnumerable<Order>> GetOrdersByFarmerIdAsync(int farmerId)
    {
        return await _context.Orders
            .Include(o => o.Crop)
            .Include(o => o.Buyer) // So the farmer knows who is buying
            .Where(o => o.Crop.SellerId == farmerId)
            .ToListAsync();
    }

    // 3. Find a specific order (needed for the Approval step)
    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Crop) // Include crop so we can check/update quantity later
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}