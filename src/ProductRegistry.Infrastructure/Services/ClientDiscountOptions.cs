using System.ComponentModel.DataAnnotations;

namespace ProductRegistry.Infrastructure.Services;

public class ClientDiscountOptions
{
    [Required] public string BaseAddress { get; set; } = null!;
    [Required] public TimeSpan Timeout { get; set; }
}