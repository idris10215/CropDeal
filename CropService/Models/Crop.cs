namespace CropService.Models;

public class Crop
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // e.g., Grain, Vegetable, Fruit
    public double Quantity { get; set; } // in kg or tons
    public decimal Price { get; set; }
    public string FarmerEmail { get; set; } = string.Empty; // To link back to our User
}