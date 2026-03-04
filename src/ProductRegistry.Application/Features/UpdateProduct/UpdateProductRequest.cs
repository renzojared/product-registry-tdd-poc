using System.Text.Json.Serialization;
using ProductRegistry.Domain.Enums;

namespace ProductRegistry.Application.Features.UpdateProduct;

public record UpdateProductRequest(
    [property: JsonIgnore] int Id,
    string Name,
    ProductStatus Status,
    int Stock,
    string Description,
    decimal Price,
    string Sku) : IRequest;