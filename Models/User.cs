namespace CropDeal.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "Farmer", "Buyer", "Admin"
    public bool isActive { get; set; } = true;

    // A User (with Role "Farmer") can have many crops
    public List<Crop> Crops { get; set; } = new();
}