using MiApp.Application.DTOs;
using MiApp.Application.Interfaces;
using MiApp.Application.UseCases;
using MiApp.Domain.Exceptions;
using MiApp.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderUseCase _createOrderUseCase;
    private readonly IOrderRepository _orderRepository;

    public OrdersController(CreateOrderUseCase createOrderUseCase, IOrderRepository orderRepository)
    {
        _createOrderUseCase = createOrderUseCase;
        _orderRepository = orderRepository;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id, ct);
        if (order is null) return NotFound();
        return Ok(order);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId, ct);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        // Mapeo del modelo HTTP al command de Application
        var command = new CreateOrderCommand(
            request.UserId,
            request.Items.Select(i => new OrderItemCommand(i.ProductId, i.Quantity)).ToList());

        try
        {
            // COMMAND: pasa por el caso de uso
            var order = await _createOrderUseCase.ExecuteAsync(command, ct);
            if (order is null)
                return NotFound(new { message = "Uno o más productos no fueron encontrados." });

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
