using CropDeal.Models;
using CropDeal.Data;

namespace CropDeal.Repositories;

public interface IOrderRepository {
    Task AddOrderAsync(Order order); 
    Task SaveChangesAsync();
}

public class OrderRepository : IOrderRepository {
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) { _context = context; }

    // This now accepts the actual Database Model
    public async Task AddOrderAsync(Order order) => await _context.Orders.AddAsync(order);
    
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}