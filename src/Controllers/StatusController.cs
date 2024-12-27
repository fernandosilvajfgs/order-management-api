using Microsoft.AspNetCore.Mvc;
using AutoMapper;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;
    private readonly IMapper _mapper;

    public StatusController(IStatusService statusService, IMapper mapper)
    {
        _statusService = statusService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> ChangeStatus([FromBody] StatusRequestDto requestDto)
    {
        var request = _mapper.Map<StatusRequest>(requestDto);
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
