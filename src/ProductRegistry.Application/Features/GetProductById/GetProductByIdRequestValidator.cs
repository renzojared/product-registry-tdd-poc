namespace ProductRegistry.Application.Features.GetProductById;

public class GetProductByIdRequestValidator : AbstractValidator<GetProductByIdRequest>
{
    public GetProductByIdRequestValidator()
    {
        RuleFor(s => s.Id)
            .GreaterThan(0)
            .WithMessage("Ingrese un valor de producto valido.");
    }
}