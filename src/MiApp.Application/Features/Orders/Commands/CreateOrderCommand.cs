using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;
using MiApp.Application.Responses;
using MiApp.Domain.Entities;

namespace MiApp.Application.Features.Orders.Commands;

/// <summary>Representa una línea de pedido dentro de un CreateOrderCommand.</summary>
public record OrderLine(Guid ProductId, int Quantity);

public record CreateOrderCommand(Guid UserId, List<OrderLine> Items) : ICommand<OrderResponse?>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse?>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderResponse?> Handle(CreateOrderCommand command, CancellationToken ct)
    {
        var order = new Order(Guid.NewGuid(), command.UserId);

        foreach (var line in command.Items)
        {
            var product = await _productRepository.GetByIdAsync(line.ProductId, ct);
            if (product is null) return null;

            order.AddItem(product, line.Quantity);
        }

        await _orderRepository.AddAsync(order, ct);

        return new OrderResponse(
            order.Id,
            order.UserId,
            order.CreatedAt,
            order.Status.ToString(),
            order.Total,
            order.Items.Select(i => new OrderItemResponse(i.Id, i.ProductId, i.UnitPrice, i.Quantity, i.Subtotal)).ToList());
    }
}
