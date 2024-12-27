public interface IPedidoRepository
{
    Task<List<Pedido>> GetAllPedidosAsync();
    Task<Pedido?> GetByCodigoAsync(string codigo);
    Task AddAsync(Pedido pedido);
    Task UpdateAsync(Pedido pedido);
    Task DeleteAsync(Pedido pedido);
}