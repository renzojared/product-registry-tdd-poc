namespace ProductRegistry.Application.Features.GetProductById;

public record GetProductByIdRequest(int Id) : IRequest<GetProductByIdResponse>;

public record GetProductByIdResponse(
    int ProductId,
    string Name,
    string StatusName,
    int Stock,
    string Description,
    decimal Price,
    decimal Discount,
    decimal FinalPrice,
    string Sku
);