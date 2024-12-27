// Controllers/PedidoController.cs
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    private readonly IMapper _mapper;

    public PedidoController(IPedidoService pedidoService, IMapper mapper)
    {
        _pedidoService = pedidoService;
        _mapper = mapper;
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

            var pedidoDto = _mapper.Map<PedidoResponseDto>(pedido);
            return Ok(pedidoDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePedido([FromBody] CreatePedidoDto createPedidoDto)
    {
        try
        {
            var pedido = _mapper.Map<Pedido>(createPedidoDto);
            var novoPedido = await _pedidoService.CreatePedidoAsync(pedido);
            var pedidoResponseDto = _mapper.Map<PedidoResponseDto>(novoPedido);
            return CreatedAtAction(nameof(GetPedido), new { codigo = novoPedido.Codigo }, pedidoResponseDto);
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
    public async Task<IActionResult> UpdatePedido(string codigo, [FromBody] UpdatePedidoDto updatePedidoDto)
    {
        try
        {
            var updatedPedido = _mapper.Map<Pedido>(updatePedidoDto);
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
