using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet("{codigo}")]
    public async Task<IActionResult> GetPedido(string codigo)
    {
        try
        {
            var pedido = await _pedidoService.GetByCodigoAsync(codigo);
            if (pedido == null)
            {
                return NotFound(new { status = "CODIGO_PEDIDO_INVALIDO" });
            }

            return Ok(pedido);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePedido([FromBody] Pedido pedido)
    {
        try
        {
            var novoPedido = await _pedidoService.CreatePedidoAsync(pedido);
            return CreatedAtAction(nameof(GetPedido), new { codigo = novoPedido.Codigo }, novoPedido);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new { status = e.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPut("{codigo}")]
    public async Task<IActionResult> UpdatePedido(string codigo, [FromBody] Pedido updatedPedido)
    {
        try
        {
            await _pedidoService.UpdatePedidoAsync(codigo, updatedPedido);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { status = e.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> DeletePedido(string codigo)
    {
        try
        {
            await _pedidoService.DeletePedidoAsync(codigo);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { status = e.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
}
