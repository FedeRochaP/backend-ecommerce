namespace MiApp.Application.DTOs;

/// <summary>DTO de entrada para el caso de uso de creación de orden.</summary>
public record CreateOrderCommand(Guid UserId, List<OrderItemCommand> Items);

/// <summary>Cada línea de la orden dentro del command.</summary>
public record OrderItemCommand(Guid ProductId, int Quantity);
