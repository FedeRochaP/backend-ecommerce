# MiApp — E-Commerce Backend API

API REST desarrollada en **ASP.NET Core (.NET 8)** siguiendo **Clean Architecture**.  
Implementa autenticación con JWT, encriptación de contraseñas con BCrypt y persistencia con Entity Framework Core + SQLite.

---

## Tecnologías

| Tecnología | Versión |
|---|---|
| .NET | 8.0 |
| ASP.NET Core | 8.0 |
| Entity Framework Core | 8.0 |
| SQLite | — |
| BCrypt.Net-Next | 4.0.3 |
| JWT Bearer | 8.0 |
| Swashbuckle (Swagger) | 6.9 |

---

## Arquitectura

El proyecto sigue **Clean Architecture** dividido en 4 capas:

```
MiApp.Domain          → Entidades, Value Objects, Excepciones, Interfaces base
MiApp.Application     → Casos de uso, DTOs, Interfaces de repositorios
MiApp.Infrastructure  → EF Core, Repositorios, JWT, BCrypt, Seed de datos
MiApp.WebApi          → Controllers, configuración, Swagger
```

### Estructura de carpetas

```
src/
├── MiApp.Domain/
│   ├── Entities/         # Product, Order, OrderItem, User, Category
│   ├── Enums/            # OrderStatus
│   ├── Exceptions/       # DomainException, InsufficientStockException
│   ├── Interfaces/       # IRepository<T> (base genérica)
│   └── ValueObjects/     # Money
├── MiApp.Application/
│   ├── DTOs/             # LoginRequest, LoginResponse, RegisterRequest
│   ├── Interfaces/       # IProductRepository, IOrderRepository, IUserRepository, ITokenService, IPasswordHasher
│   └── UseCases/         # LoginUseCase, RegisterUseCase
├── MiApp.Infrastructure/
│   ├── Migrations/
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/   # Fluent API para cada entidad
│   ├── Repositories/         # ProductRepository, OrderRepository, UserRepository
│   ├── Services/             # JwtTokenService, BCryptPasswordHasher
│   ├── DataSeeder.cs
│   └── InfrastructureServiceExtensions.cs
└── MiApp.WebApi/
    ├── Controllers/      # AuthController, ProductsController, OrdersController
    ├── Models/           # Request models
    └── Program.cs
```

---

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## Cómo ejecutar

```bash
# 1. Clonar / abrir el proyecto
cd MiApp/src/MiApp.WebApi

# 2. (Opcional) Borrar la BD para iniciar desde cero
rm ecommerce.db

# 3. Iniciar la API
dotnet run
```

Al arrancar, la app:
1. Aplica las migraciones automáticamente (crea `ecommerce.db` si no existe).
2. Inserta datos de prueba si la base está vacía.

Swagger UI disponible en: **http://localhost:5213/swagger**

---

## Datos de prueba (Seed)

| Email | Contraseña | Rol |
|---|---|---|
| admin@miapp.com | Admin123! | Admin |
| user@miapp.com | User123! | User |

Se insertan también **6 productos** distribuidos en 3 categorías: Electrónica, Ropa y Hogar.

---

## Endpoints

### Auth

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| POST | `/api/auth/login` | Obtener token JWT | No |
| POST | `/api/auth/register` | Registrar usuario | No |

### Products

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| GET | `/api/products` | Listar todos | No |
| GET | `/api/products/{id}` | Obtener por Id | No |
| GET | `/api/products/search?name=...` | Buscar por nombre | No |
| POST | `/api/products` | Crear producto | Admin |
| PUT | `/api/products/{id}` | Actualizar producto | Admin |
| DELETE | `/api/products/{id}` | Eliminar producto | Admin |

### Orders

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| GET | `/api/orders/{id}` | Obtener orden con items | User |
| GET | `/api/orders/user/{userId}` | Órdenes de un usuario | User |
| POST | `/api/orders` | Crear orden | User |

---

## Autenticación

1. Hacer `POST /api/auth/login` con las credenciales.
2. Copiar el `token` de la respuesta.
3. En Swagger: click en **Authorize** → ingresar `Bearer <token>`.

---

## Configuración (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ecommerce.db"
  },
  "Jwt": {
    "Key": "MiAppSuperSecretKey2026MiAppSuperSecretKey2026",
    "Issuer": "MiApp",
    "Audience": "MiApp",
    "ExpiresInMinutes": "60"
  }
}
```
