namespace ProductRegistry.Application.Features.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no debe superar los 100 caracteres.");

        RuleFor(v => v.Status)
            .IsInEnum().WithMessage("Se debe proporcionar un estado válido (0 Inactivo, 1 Activo).");

        RuleFor(v => v.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("La descripción es requerida.")
            .MaximumLength(500).WithMessage("La descripción no debe superar los 500 caracteres.");

        RuleFor(v => v.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero.");

        RuleFor(v => v.Sku)
            .Length(8).WithMessage("El SKU debe tener exactamente 8 caracteres.");
    }
}