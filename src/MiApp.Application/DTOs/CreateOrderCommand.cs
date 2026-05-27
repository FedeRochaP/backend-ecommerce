namespace MiApp.Application.DTOs;

/// <summary>DTO de entrada legado para creación de orden (reemplazado por CQRS CreateOrderCommand).</summary>
public record CreateOrderDto(Guid UserId, List<OrderItemDto> Items);

/// <summary>Cada línea de la orden dentro del DTO legado.</summary>
public record OrderItemDto(Guid ProductId, int Quantity);
