using DotEmilu.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductRegistry.Domain;
using ProductRegistry.Infrastructure.DataAccess;
using ProductRegistry.Infrastructure.DataAccess.Repositories;

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
            .AddScoped<ICommands, Commands>();

    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RegistryContext>();

        await context.Database.MigrateAsync();
    }
}