using ProductRegistry.Domain;

namespace ProductRegistry.Application.Features.UpdateProduct;

public class UpdateProductHandler(IVerifier<UpdateProductRequest> verifier, ICommands commands)
    : Handler<UpdateProductRequest>(verifier)
{
    private readonly IVerifier<UpdateProductRequest> _verifier = verifier;

    protected override async Task HandleUseCaseAsync(UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await commands.Products
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (product is null)
        {
            _verifier.AddError(nameof(UpdateProductRequest.Id), "El producto no existe.");
            return;
        }

        product.Update(
            request.Name,
            request.Status,
            request.Stock,
            request.Description,
            request.Price,
            request.Sku
        );

        await commands.SaveChangesAsync(cancellationToken);
    }
}