using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;
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
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
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
        try
        {
            var order = new Order(Guid.NewGuid(), request.UserId);

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, ct);
                if (product is null)
                    return NotFound(new { message = $"Producto '{item.ProductId}' no encontrado." });

                order.AddItem(product, item.Quantity);
            }

            await _orderRepository.AddAsync(order, ct);
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
