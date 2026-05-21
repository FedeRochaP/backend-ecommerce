using MiApp.Domain.Exceptions;

namespace MiApp.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public Guid CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Product() { }

    public Product(Guid id, string name, string description, decimal price, int stock, Guid categoryId)
    {
        if (id == Guid.Empty) throw new ArgumentException("El Id no puede ser vacío.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre no puede ser vacío.", nameof(name));
        if (price < 0) throw new ArgumentException("El precio no puede ser negativo.", nameof(price));
        if (stock < 0) throw new ArgumentException("El stock no puede ser negativo.", nameof(stock));

        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre no puede ser vacío.", nameof(name));
        if (price < 0) throw new ArgumentException("El precio no puede ser negativo.", nameof(price));
        if (stock < 0) throw new ArgumentException("El stock no puede ser negativo.", nameof(stock));
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0) throw new ArgumentException("El precio no puede ser negativo.", nameof(newPrice));
        Price = newPrice;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));
        if (Stock < quantity) throw new InsufficientStockException(Name, quantity, Stock);
        Stock -= quantity;
    }
}
