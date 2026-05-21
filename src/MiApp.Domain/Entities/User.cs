namespace MiApp.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public User(Guid id, string email, string name, string passwordHash, string role = "User")
    {
        if (id == Guid.Empty) throw new ArgumentException("El Id no puede ser vacío.", nameof(id));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El email no puede ser vacío.", nameof(email));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre no puede ser vacío.", nameof(name));
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("El hash de contraseña no puede ser vacío.", nameof(passwordHash));

        Id = id;
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
}
