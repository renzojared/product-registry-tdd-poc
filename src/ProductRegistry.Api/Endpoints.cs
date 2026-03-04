using ProductRegistry.Application.Features.CreateProduct;
using ProductRegistry.Application.Features.UpdateProduct;

namespace ProductRegistry.Api;

public static class Endpoints
{
    internal static WebApplication MapEndpoints(this WebApplication application)
    {
        application
            .MapGroup("api/product")
            .WithTags("Products")
            .MapCreateProduct()
            .MapUpdateProduct();

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

    private static RouteGroupBuilder MapUpdateProduct(this RouteGroupBuilder builder)
    {
        builder
            .MapPut("{id:int}",
                (int id, UpdateProductRequest request, HttpHandler<UpdateProductRequest> handler,
                        CancellationToken cancellationToken) =>
                    AsDelegate.ForAsync<UpdateProductRequest>(TypedResults.NoContent)(request with { Id = id },
                        handler,
                        cancellationToken))
            .WithName("UpdateProduct")
            .WithSummary("Actualiza un producto existente")
            .WithDescription("Actualiza un producto. Retorna 204 No Content.")
            .Produces(StatusCodes.Status204NoContent)
            .ProduceProblems();

        return builder;
    }

    private static RouteHandlerBuilder ProduceProblems(this RouteHandlerBuilder builder)
        => builder
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError);
}