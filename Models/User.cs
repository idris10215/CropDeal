namespace CropDeal.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; 
    public bool isActive { get; set; } = true;

    public List<Crop> Crops { get; set; } = new();
}