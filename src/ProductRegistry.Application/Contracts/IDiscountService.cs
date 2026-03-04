namespace ProductRegistry.Application.Contracts;

public interface IDiscountService
{
    Task<short?> GetDiscountAsync(int productId, CancellationToken cancellationToken = default);
}