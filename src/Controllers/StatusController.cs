using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpPost]
    public async Task<IActionResult> ChangeStatus([FromBody] StatusRequest request)
    {
        var (success, message, statusList) = await _statusService.ChangeStatusAsync(request);

        if (!success && message == "CODIGO_PEDIDO_INVALIDO")
        {
            return NotFound(new { pedido = request.Pedido, status = new[] { message } });
        }

        if (!success)
        {
            return BadRequest(new { pedido = request.Pedido, status = new[] { message } });
        }

        return Ok(new { pedido = request.Pedido, status = statusList });
    }
}
