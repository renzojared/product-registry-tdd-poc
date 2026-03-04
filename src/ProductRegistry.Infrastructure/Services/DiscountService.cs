using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using ProductRegistry.Application.Contracts;

namespace ProductRegistry.Infrastructure.Services;

public partial class DiscountService(HttpClient httpClient, ILogger<DiscountService> logger) : IDiscountService
{
    public async Task<short?> GetDiscountAsync(int productId, CancellationToken cancellationToken = default)
    {
        try
        {
            LogRetrievingDiscount(logger, productId);

            var response = await httpClient.GetAsync($"/discount?productId={productId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                LogFailedRetrieveDiscount(logger, productId, response.StatusCode);
                return null;
            }

            var discountRecord = await response.Content.ReadFromJsonAsync<DiscountResponse>(cancellationToken);
            return discountRecord?.DiscountPercentage;
        }
        catch (HttpRequestException ex)
        {
            LogHttpException(logger, ex, productId);
            return null;
        }
    }

    [LoggerMessage(Level = LogLevel.Information,
        Message = "Retrieving discount from external service for product {ProductId}")]
    private static partial void LogRetrievingDiscount(ILogger logger, int productId);

    [LoggerMessage(Level = LogLevel.Warning,
        Message = "Failed to retrieve discount for product {ProductId}. Status Code: {StatusCode}")]
    private static partial void LogFailedRetrieveDiscount(ILogger logger, int productId,
        System.Net.HttpStatusCode statusCode);

    [LoggerMessage(Level = LogLevel.Error,
        Message = "HTTP exception occurred while retrieving discount for product {ProductId}")]
    private static partial void LogHttpException(ILogger logger, Exception ex, int productId);

    private sealed record DiscountResponse(short DiscountPercentage);
}