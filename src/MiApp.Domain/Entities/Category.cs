namespace MiApp.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Category() { }

    public Category(Guid id, string name)
    {
        if (id == Guid.Empty) throw new ArgumentException("El Id no puede ser vacío.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre no puede ser vacío.", nameof(name));

        Id = id;
        Name = name;
    }
}
