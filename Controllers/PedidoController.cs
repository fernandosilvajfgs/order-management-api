using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly PedidoDbContext _context;

    public PedidoController(PedidoDbContext context)
    {
        _context = context;
    }

    [HttpGet("{codigo}")]
    public async Task<IActionResult> GetPedido(string codigo)
    {
        var pedido = await _context.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Codigo == codigo);
        if (pedido == null) return NotFound(new { status = "CODIGO_PEDIDO_INVALIDO" });

        return Ok(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePedido([FromBody] Pedido pedido)
    {
        if (_context.Pedidos.Any(p => p.Codigo == pedido.Codigo))
        {
            return BadRequest(new { status = "CODIGO_PEDIDO_DUPLICADO" });
        }

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPedido), new { codigo = pedido.Codigo }, pedido);
    }

    [HttpPut("{codigo}")]
    public async Task<IActionResult> UpdatePedido(string codigo, [FromBody] Pedido updatedPedido)
    {
        var pedido = await _context.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Codigo == codigo);
        if (pedido == null) return NotFound(new { status = "CODIGO_PEDIDO_INVALIDO" });

        pedido.Itens = updatedPedido.Itens;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> DeletePedido(string codigo)
    {
        var pedido = await _context.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Codigo == codigo);
        if (pedido == null) return NotFound(new { status = "CODIGO_PEDIDO_INVALIDO" });

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
