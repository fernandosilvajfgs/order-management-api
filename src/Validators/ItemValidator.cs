using FluentValidation;

public class ItemValidator : AbstractValidator<Item>
{
    public ItemValidator()
    {
        RuleFor(i => i.Descricao)
            .NotEmpty()
            .WithMessage("O campo 'Descricao' é obrigatório.");

        RuleFor(i => i.PrecoUnitario)
            .GreaterThan(0)
            .WithMessage("O campo 'PrecoUnitario' deve ser maior que zero.");

        RuleFor(i => i.Qtd)
            .GreaterThan(0)
            .WithMessage("O campo 'Qtd' deve ser maior que zero.");
    }
}
