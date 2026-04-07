namespace CropDeal.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    
    public int Quantity { get; set; } 

    public int CropId { get; set; }
    public Crop? Crop { get; set; }

    public int BuyerId { get; set; }
    public User? Buyer { get; set; }
}