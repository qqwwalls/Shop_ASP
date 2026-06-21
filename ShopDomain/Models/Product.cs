namespace ShopDomain.Models;

public class Product
{
    private static int _nextId = 1;

    public Product()
    {
        Id = _nextId++;
    }

    public int Id { get; set; }
    public string? Title { get; set; }
    public float Price { get; set; }
    public int Count { get; set; }
    public float Discount { get; set; }
    public bool IsActive { get; set; }
}
