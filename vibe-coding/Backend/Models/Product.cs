namespace Backend.Models;

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? Brands { get; set; }
    public string? Categories { get; set; }
}
