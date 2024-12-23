using Microsoft.EntityFrameworkCore;

public class PedidoDbContext : DbContext
{
    public PedidoDbContext(DbContextOptions<PedidoDbContext> options) : base(options) { }

    public DbSet<Pedido> Pedidos { get; set; }
}