using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;
using MiApp.Application.Responses;

namespace MiApp.Application.Features.Orders.Queries;

public record GetOrderByIdQuery(Guid Id) : IQuery<OrderResponse?>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        => _orderRepository = orderRepository;

    public async Task<OrderResponse?> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(query.Id, ct);
        if (order is null) return null;

        return new OrderResponse(
            order.Id,
            order.UserId,
            order.CreatedAt,
            order.Status.ToString(),
            order.Total,
            order.Items.Select(i => new OrderItemResponse(i.Id, i.ProductId, i.UnitPrice, i.Quantity, i.Subtotal)).ToList());
    }
}
