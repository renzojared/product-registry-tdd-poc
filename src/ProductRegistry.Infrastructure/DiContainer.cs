using DotEmilu.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductRegistry.Domain;
using ProductRegistry.Infrastructure.DataAccess;
using ProductRegistry.Infrastructure.DataAccess.Repositories;
using ProductRegistry.Infrastructure.Services;

namespace ProductRegistry.Infrastructure;

public static class DiContainer
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        Assembly apiAssembly)
        => services
            .AddSoftDeleteInterceptor()
            .AddAuditableEntityInterceptors(apiAssembly)
            .AddDbContext<RegistryContext>((sp, options) => options
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"), postgres =>
                    postgres.MapEnum<ProductStatus>())
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>()))
            .AddScoped<IQueries, Queries>()
            .AddScoped<ICommands, Commands>()
            .ConfigureDiscountClient();

    private static IServiceCollection ConfigureDiscountClient(this IServiceCollection services)
    {
        services
            .AddOptions<ClientDiscountOptions>()
            .BindConfiguration(nameof(ClientDiscountOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient<ProductRegistry.Application.Contracts.IDiscountService, DiscountService>((sp, options) =>
        {
            var service = sp.GetRequiredService<IOptionsMonitor<ClientDiscountOptions>>().CurrentValue;
            options.BaseAddress = new Uri(service.BaseAddress);
            options.Timeout = service.Timeout;
        });

        return services;
    }

    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RegistryContext>();

        await context.Database.MigrateAsync();
    }
}