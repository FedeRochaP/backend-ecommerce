using MiApp.Domain.Enums;

namespace MiApp.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Total { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    public Order(Guid id, Guid userId)
    {
        if (id == Guid.Empty) throw new ArgumentException("El Id no puede ser vacío.", nameof(id));
        if (userId == Guid.Empty) throw new ArgumentException("El UserId no puede ser vacío.", nameof(userId));

        Id = id;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        Total = 0;
    }

    public void AddItem(Product product, int quantity)
    {
        if (product is null) throw new ArgumentNullException(nameof(product));
        if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));

        product.ReduceStock(quantity);

        var item = new OrderItem(Guid.NewGuid(), Id, product.Id, product.Price, quantity);
        _items.Add(item);
        Total += item.Subtotal;
    }
}
