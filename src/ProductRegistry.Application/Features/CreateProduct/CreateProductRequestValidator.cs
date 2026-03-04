namespace ProductRegistry.Application.Features.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(v => v.Status)
            .IsInEnum();

        RuleFor(v => v.Stock)
            .GreaterThanOrEqualTo(0);

        RuleFor(v => v.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(v => v.Price)
            .GreaterThan(0);

        RuleFor(v => v.Sku)
            .Length(8);
    }
}