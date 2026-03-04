using ProductRegistry.Domain;
using ProductRegistry.Domain.Entities;

namespace ProductRegistry.Application.Features.CreateProduct;

public class CreateProductHandler(IVerifier<CreateProductRequest> verifier, ICommands commands)
    : Handler<CreateProductRequest, int>(verifier)
{
    protected override async Task<int> HandleUseCaseAsync(CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = new Product(
            request.Name,
            request.Status,
            request.Stock,
            request.Description,
            request.Price,
            request.Sku
        );

        await commands.Products.AddAsync(product, cancellationToken);

        return product.Id;
    }
}