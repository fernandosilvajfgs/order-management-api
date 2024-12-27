public class StatusService : IStatusService
{
    private readonly IPedidoRepository _pedidoRepository;

    public StatusService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<(bool Success, string? Message, IEnumerable<string> StatusList)> ChangeStatusAsync(StatusRequest request)
    {
        var pedido = await _pedidoRepository.GetByCodigoAsync(request.Pedido);

        if (pedido == null)
        {
            return (false, "CODIGO_PEDIDO_INVALIDO", Enumerable.Empty<string>());
        }

        var statusList = new List<string>();

        if (request.Status == "REPROVADO")
        {
            statusList.Add("REPROVADO");
        }
        else
        {
            var totalValue = pedido.Itens.Sum(i => i.PrecoUnitario * i.Qtd);
            var totalItems = pedido.Itens.Sum(i => i.Qtd);

            if (request.ItensAprovados == totalItems && request.ValorAprovado == totalValue)
            {
                statusList.Add("APROVADO");
            }
            if (request.ValorAprovado < totalValue)
            {
                statusList.Add("APROVADO_VALOR_A_MENOR");
            }
            if (request.ValorAprovado > totalValue)
            {
                statusList.Add("APROVADO_VALOR_A_MAIOR");
            }
            if (request.ItensAprovados < totalItems)
            {
                statusList.Add("APROVADO_QTD_A_MENOR");
            }
            if (request.ItensAprovados > totalItems)
            {
                statusList.Add("APROVADO_QTD_A_MAIOR");
            }
        }

        return (true, null, statusList);
    }
}
