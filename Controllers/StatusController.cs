using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly PedidoDbContext _context;

    public StatusController(PedidoDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> ChangeStatus([FromBody] StatusRequest request)
    {
        var pedido = await _context.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Codigo == request.Pedido);

        if (pedido == null)
        {
            return NotFound(new { pedido = request.Pedido, status = new[] { "CODIGO_PEDIDO_INVALIDO" } });
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

        return Ok(new { pedido = request.Pedido, status = statusList });

    }
}