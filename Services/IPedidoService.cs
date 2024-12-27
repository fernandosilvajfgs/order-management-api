public interface IPedidoService
{
    Task<Pedido?> GetByCodigoAsync(string codigo);
    Task<Pedido> CreatePedidoAsync(Pedido pedido);
    Task UpdatePedidoAsync(string codigo, Pedido pedido);
    Task DeletePedidoAsync(string codigo);
}