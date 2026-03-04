using ProductRegistry.Domain.Enums;

namespace ProductRegistry.Application.Features.CreateProduct;

public record CreateProductRequest(
    string Name,
    ProductStatus Status,
    int Stock,
    string Description,
    decimal Price,
    string Sku
) : IRequest<int>;