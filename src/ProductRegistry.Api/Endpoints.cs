using ProductRegistry.Application.Features.CreateProduct;

namespace ProductRegistry.Api;

public static class Endpoints
{
    internal static WebApplication MapEndpoints(this WebApplication application)
    {
        application
            .MapGroup("api/product")
            .WithTags("Products")
            .MapCreateProduct();

        return application;
    }

    private static RouteGroupBuilder MapCreateProduct(this RouteGroupBuilder builder)
    {
        builder
            .MapPost(string.Empty, AsDelegate.ForAsync<CreateProductRequest, int>())
            .WithName("CreateProduct")
            .WithSummary("Crea un nuevo producto en el catálogo")
            .WithDescription(
                "Registra un nuevo producto tras validar reglas de negocio. Retorna el ID del producto creado.")
            .Produces<int>()
            .ProduceProblems();

        return builder;
    }

    private static RouteHandlerBuilder ProduceProblems(this RouteHandlerBuilder builder)
        => builder
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError);
}