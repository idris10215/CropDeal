namespace CropDeal.Models;

public class OrderCreateDto
{
    public int CropId { get; set; }
    public int BuyerId { get; set; }
    public int Quantity { get; set; } 
    public decimal TotalAmount { get; set; }
}