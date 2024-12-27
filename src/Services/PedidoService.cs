public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<List<Pedido>> GetAllPedidosAsync()
    {
        return await _pedidoRepository.GetAllPedidosAsync();
    }

    public async Task<Pedido?> GetByCodigoAsync(string codigo)
    {
        return await _pedidoRepository.GetByCodigoAsync(codigo);
    }

    public async Task<Pedido> CreatePedidoAsync(Pedido pedido)
    {
        var pedidoExistente = await _pedidoRepository.GetByCodigoAsync(pedido.Codigo);
        if (pedidoExistente != null)
        {
            throw new InvalidOperationException("CODIGO_PEDIDO_DUPLICADO");
        }

        await _pedidoRepository.AddAsync(pedido);
        return pedido;
    }

    public async Task UpdatePedidoAsync(string codigo, Pedido pedidoAtualizado)
    {
        var pedidoExistente = await _pedidoRepository.GetByCodigoAsync(codigo);
        if (pedidoExistente == null)
        {
            throw new KeyNotFoundException("CODIGO_PEDIDO_INVALIDO");
        }
        pedidoExistente.Itens = pedidoAtualizado.Itens;

        await _pedidoRepository.UpdateAsync(pedidoExistente);
    }

    public async Task DeletePedidoAsync(string codigo)
    {
        var pedidoExistente = await _pedidoRepository.GetByCodigoAsync(codigo);
        if (pedidoExistente == null)
        {
            throw new KeyNotFoundException("CODIGO_PEDIDO_INVALIDO");
        }

        await _pedidoRepository.DeleteAsync(pedidoExistente);
    }
}
