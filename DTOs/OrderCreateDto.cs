namespace CropDeal.Models;

public class OrderCreateDto
{
    public int CropId { get; set; }
    public int BuyerId { get; set; }
    public int Quantity { get; set; } // Added this so they can choose how much to buy
    public decimal TotalAmount { get; set; }
}