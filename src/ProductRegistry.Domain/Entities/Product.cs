using ProductRegistry.Domain.Enums;

namespace ProductRegistry.Domain.Entities;

public sealed class Product : BaseAuditableEntity<int, Guid>
{
    public string Name { get; private set; }

    public ProductStatus Status { get; private set; }

    public int Stock { get; private set; }

    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public string Sku { get; private set; }

    private Product()
    {
        Name = null!;
        Description = null!;
        Sku = null!;
    }

    public Product(string name, ProductStatus status, int stock, string description, decimal price, string sku)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegative(stock);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);

        Name = name;
        Status = status;
        Stock = stock;
        Description = description;
        Price = price;
        Sku = sku;
    }

    /// <summary>
    /// The final price after applying an external discount percentage [0–100].
    /// </summary>
    public decimal CalculateFinalPrice(decimal discountPercentage) =>
        Price * (100m - discountPercentage) / 100m;
}