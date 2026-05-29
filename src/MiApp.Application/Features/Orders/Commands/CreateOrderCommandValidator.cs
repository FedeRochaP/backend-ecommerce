using FluentValidation;

namespace MiApp.Application.Features.Orders.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es obligatorio.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("La orden debe contener al menos un item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("El ProductId de cada item es obligatorio.");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("La cantidad de cada item debe ser mayor a cero.");
        });
    }
}
