namespace MiApp.Application.Responses;

public record OrderItemResponse(
    Guid Id,
    Guid ProductId,
    decimal UnitPrice,
    int Quantity,
    decimal Subtotal);

public record OrderResponse(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    string Status,
    decimal Total,
    List<OrderItemResponse> Items);
