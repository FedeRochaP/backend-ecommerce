using MiApp.Application.DTOs;
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;

namespace MiApp.Application.UseCases;

/// <summary>
/// COMMAND: Crea una nueva orden, descuenta stock y persiste.
/// Retorna la orden creada, o null si algún producto no existe.
/// </summary>
public class CreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderUseCase(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Order?> ExecuteAsync(CreateOrderCommand command, CancellationToken ct = default)
    {
        var order = new Order(Guid.NewGuid(), command.UserId);

        foreach (var item in command.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, ct);
            if (product is null) return null;

            order.AddItem(product, item.Quantity); // aplica reglas de dominio + descuenta stock
        }

        await _orderRepository.AddAsync(order, ct);
        return order;
    }
}
