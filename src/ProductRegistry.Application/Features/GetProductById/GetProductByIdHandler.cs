using Microsoft.Extensions.Caching.Memory;
using ProductRegistry.Application.Contracts;
using ProductRegistry.Domain;
using ProductRegistry.Domain.Enums;

namespace ProductRegistry.Application.Features.GetProductById;

public class GetProductByIdHandler(
    IVerifier<GetProductByIdRequest> verifier,
    IQueries queries,
    IMemoryCache memoryCache,
    IDiscountService discountService) : Handler<GetProductByIdRequest, GetProductByIdResponse>(verifier)
{

    protected override async Task<GetProductByIdResponse?> HandleUseCaseAsync(GetProductByIdRequest request,
        CancellationToken cancellationToken)
    {
        var product = await queries.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null) return null;

        var discountTask = discountService.GetDiscountAsync(request.Id, cancellationToken);
        var productStatusTask = GetStatusName(product.Status);

        await Task.WhenAll(discountTask, productStatusTask);

        var discount = discountTask.Result ?? 0;
        var finalPrice = product.CalculateFinalPrice(discount);

        return new GetProductByIdResponse(
            product.Id,
            product.Name,
            productStatusTask.Result,
            product.Stock,
            product.Description,
            product.Price,
            discount,
            finalPrice,
            product.Sku
        );
    }

    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Este metodo permite obtener el nombre del estado
    /// NOTA: Dado que el estado en la base de datos es un enum <see cref="ProductStatus"/>, 
    /// en la práctica se podría usar nativamente Enum.GetName(status), el nameof(ProductStatus.Active), 
    /// o un simple ".ToString()", eliminando por completo la necesidad de un caché.
    /// </summary>
    /// <param name="status">El estado del producto</param>
    /// <returns>El nombre en texto del estado</returns>
    private async Task<string> GetStatusName(ProductStatus status)
    {
        var statusDictionary = await memoryCache.GetOrCreateAsync("ProductStatusDictionary", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            var dict = new Dictionary<int, string>
            {
                { 1, "Active" },
                { 0, "Inactive" }
            };
            return Task.FromResult(dict);
        });

        return statusDictionary != null && statusDictionary.TryGetValue((int)status, out var name)
            ? name
            : status.ToString();
    }
}