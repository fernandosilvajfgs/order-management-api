using FluentValidation;

public class UpdatePedidoDtoValidator : AbstractValidator<UpdatePedidoDto>
{
    public UpdatePedidoDtoValidator()
    {
        RuleFor(p => p.Itens)
            .NotNull()
            .NotEmpty()
            .WithMessage("O pedido precisa de ter pelo menos 1 item.");

        RuleForEach(p => p.Itens).SetValidator(new ItemDtoValidator());
    }
}
