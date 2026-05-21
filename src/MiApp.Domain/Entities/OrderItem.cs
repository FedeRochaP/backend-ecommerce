namespace MiApp.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    // Propiedad calculada, no persistida en la base de datos
    public decimal Subtotal => UnitPrice * Quantity;

    private OrderItem() { }

    public OrderItem(Guid id, Guid orderId, Guid productId, decimal unitPrice, int quantity)
    {
        if (id == Guid.Empty) throw new ArgumentException("El Id no puede ser vacío.", nameof(id));
        if (unitPrice < 0) throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(unitPrice));
        if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));

        Id = id;
        OrderId = orderId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
