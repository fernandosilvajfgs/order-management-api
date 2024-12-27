using Microsoft.EntityFrameworkCore;

public class PedidoRepository : IPedidoRepository
{
    private readonly PedidoDbContext _context;

    public PedidoRepository(PedidoDbContext context)
    {
        _context = context;
    }
    public async Task<List<Pedido>> GetAllAsync()
    {
        return await _context.Pedidos.Include(p => p.Itens).ToListAsync();
    }

    public async Task<Pedido?> GetByCodigoAsync(string codigo)
    {
        return await _context.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Codigo == codigo);
    }

    public async Task AddAsync(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Pedido pedido)
    {
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
    }
}