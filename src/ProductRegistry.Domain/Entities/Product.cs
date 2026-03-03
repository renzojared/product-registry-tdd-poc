using ProductRegistry.Domain.Enums;

namespace ProductRegistry.Domain.Entities;

public sealed class Product : BaseAuditableEntity<int, Guid>
{
    public string Name { get; set; } = string.Empty;

    public ProductStatus Status { get; set; }

    public int Stock { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// The final price after applying an external discount percentage [0–100].
    /// </summary>
    public decimal CalculateFinalPrice(decimal discountPercentage) =>
        Price * (100m - discountPercentage) / 100m;
}