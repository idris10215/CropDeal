namespace CropDeal.Models;

public class Crop
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    // Relationship: The User (Farmer) who posted this crop
    public int SellerId { get; set; } 
    public User? Seller { get; set; }
}