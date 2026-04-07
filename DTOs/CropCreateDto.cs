namespace CropDeal.Models;

public class CropCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int SellerId { get; set; } 
}