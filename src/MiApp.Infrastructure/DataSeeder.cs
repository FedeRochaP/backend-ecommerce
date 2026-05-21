using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;
using MiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiApp.Infrastructure;

public static class DataSeeder
{
    // GUIDs fijos de las categorías (deben coincidir con CategoryConfiguration)
    private static readonly Guid ElectronicaId = new("a1b2c3d4-0000-0000-0000-000000000001");
    private static readonly Guid RopaId        = new("a1b2c3d4-0000-0000-0000-000000000002");
    private static readonly Guid HogarId       = new("a1b2c3d4-0000-0000-0000-000000000003");

    public static async Task SeedAsync(ApplicationDbContext context, IPasswordHasher hasher)
    {
        await SeedUsersAsync(context, hasher);
        await SeedProductsAsync(context);
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context, IPasswordHasher hasher)
    {
        if (await context.Users.AnyAsync()) return;

        var users = new List<User>
        {
            new(new Guid("b1b2b3b4-0000-0000-0000-000000000001"),
                "admin@miapp.com", "Administrador",
                hasher.Hash("Admin123!"), "Admin"),

            new(new Guid("b1b2b3b4-0000-0000-0000-000000000002"),
                "user@miapp.com", "Usuario Común",
                hasher.Hash("User123!"))
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync()) return;

        var products = new List<Product>
        {
            new(new Guid("c1c2c3c4-0000-0000-0000-000000000001"),
                "iPhone 15", "Smartphone Apple 128GB", 1299.99m, 50, ElectronicaId),

            new(new Guid("c1c2c3c4-0000-0000-0000-000000000002"),
                "Samsung Galaxy S24", "Smartphone Samsung 256GB", 999.99m, 35, ElectronicaId),

            new(new Guid("c1c2c3c4-0000-0000-0000-000000000003"),
                "Remera básica", "Remera de algodón 100%", 19.99m, 200, RopaId),

            new(new Guid("c1c2c3c4-0000-0000-0000-000000000004"),
                "Campera de abrigo", "Campera invierno impermeable", 89.99m, 60, RopaId),

            new(new Guid("c1c2c3c4-0000-0000-0000-000000000005"),
                "Silla de oficina", "Silla ergonómica con ruedas", 299.99m, 30, HogarId),

            new(new Guid("c1c2c3c4-0000-0000-0000-000000000006"),
                "Lámpara LED", "Lámpara de escritorio regulable", 49.99m, 100, HogarId),
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}
