public interface IPedidoRepository
{
    Task<Pedido?> GetByCodigoAsync(string codigo);
    Task AddAsync(Pedido pedido);
    Task UpdateAsync(Pedido pedido);
    Task DeleteAsync(Pedido pedido);
}