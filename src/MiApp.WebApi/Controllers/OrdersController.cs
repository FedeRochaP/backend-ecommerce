using MediatR;
using MiApp.Application.Features.Orders.Commands;
using MiApp.Application.Features.Orders.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender) => _sender = sender;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetOrderByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct)
        => Ok(await _sender.Send(new GetOrdersByUserQuery(userId), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        if (result is null)
            return NotFound(new { message = "Uno o más productos no fueron encontrados." });

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
