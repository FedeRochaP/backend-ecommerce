namespace MiApp.WebApi.Models;

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId);

public record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int Stock);

public record CreateOrderRequest(
    Guid UserId,
    List<CreateOrderItemRequest> Items);

public record CreateOrderItemRequest(
    Guid ProductId,
    int Quantity);
