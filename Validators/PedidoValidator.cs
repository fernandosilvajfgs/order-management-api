using FluentValidation;

public class PedidoValidator : AbstractValidator<Pedido>
{
    public PedidoValidator()
    {
        RuleFor(p => p.Codigo)
            .NotEmpty()
            .WithMessage("O campo 'Codigo' é obrigatório.");

        RuleFor(p => p.Itens)
            .NotNull()
            .NotEmpty()
            .WithMessage("O pedido precisa de ter pelo menos 1 item.");

        RuleForEach(p => p.Itens).SetValidator(new ItemValidator());
    }
}
