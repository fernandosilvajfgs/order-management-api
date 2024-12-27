using FluentValidation;

public class StatusRequestValidator : AbstractValidator<StatusRequest>
{
    public StatusRequestValidator()
    {
        RuleFor(s => s.Status)
            .NotEmpty()
            .WithMessage("O campo 'Status' é obrigatório.");

        RuleFor(s => s.ItensAprovados)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O campo 'ItensAprovados' não pode ser negativo.");

        RuleFor(s => s.ValorAprovado)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O campo 'ValorAprovado' não pode ser negativo.");

        RuleFor(s => s.Pedido)
            .NotEmpty()
            .WithMessage("O campo 'Pedido' é obrigatório.");
    }
}
