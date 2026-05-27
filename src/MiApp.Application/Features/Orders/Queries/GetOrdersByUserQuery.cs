using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;
using MiApp.Application.Responses;

namespace MiApp.Application.Features.Orders.Queries;

public record GetOrdersByUserQuery(Guid UserId) : IQuery<IEnumerable<OrderResponse>>;

public class GetOrdersByUserQueryHandler : IRequestHandler<GetOrdersByUserQuery, IEnumerable<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByUserQueryHandler(IOrderRepository orderRepository)
        => _orderRepository = orderRepository;

    public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersByUserQuery query, CancellationToken ct)
    {
        var orders = await _orderRepository.GetByUserIdAsync(query.UserId, ct);

        // Los items no se cargan en la consulta de lista; usar GetOrderByIdQuery para detalle con items.
        return orders.Select(order => new OrderResponse(
            order.Id,
            order.UserId,
            order.CreatedAt,
            order.Status.ToString(),
            order.Total,
            []));
    }
}
