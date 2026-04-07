namespace CropDeal.Models;

public class CropReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string SellerName { get; set; } = string.Empty; 
}