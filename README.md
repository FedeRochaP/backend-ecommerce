# MiApp — E-Commerce Backend API

API REST desarrollada en **ASP.NET Core (.NET 8)** siguiendo **Clean Architecture** con patrón **CQRS**.  
Implementa autenticación con JWT, encriptación de contraseñas con BCrypt y persistencia con Entity Framework Core + SQLite.

---

## Tecnologías

| Tecnología | Versión |
|---|---|
| .NET | 8.0 |
| ASP.NET Core | 8.0 |
| Entity Framework Core | 8.0 |
| SQLite | — |
| MediatR | 12.4.1 |
| BCrypt.Net-Next | 4.0.3 |
| JWT Bearer | 8.0 |
| Swashbuckle (Swagger) | 6.9 |
| xUnit | 2.9 |
| Moq | 4.20 |

---

## Arquitectura

El proyecto sigue **Clean Architecture** con patrón **CQRS** (Command Query Responsibility Segregation), dividido en 4 capas:

```
MiApp.Domain          → Entidades, Value Objects, Excepciones, Interfaces base
MiApp.Application     → CQRS (Commands, Queries, Handlers), DTOs, Interfaces de repositorios
MiApp.Infrastructure  → EF Core, Repositorios, JWT, BCrypt, Seed de datos
MiApp.WebApi          → Controllers, configuración, Swagger
```

### Flujo CQRS con MediatR

```
HTTP Request
    ↓
Controller
    ↓  ISender.Send(command/query)
MediatR (dispatcher)
    ↓  resuelve IRequestHandler<TRequest, TResponse>
Handler (Application/Features)
    ↓  usa interfaces de repositorio
Repository (Infrastructure)
    ↓
Base de datos (SQLite)
```

Los controllers **nunca** acceden directamente a repositorios ni a entidades de dominio.  
Toda la lógica de negocio vive en los handlers.  
MediatR actúa como mediador, desacoplando controllers de handlers mediante `ISender`.

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
│   ├── Common/           # IQuery<T>, ICommand<T> (extienden IRequest<T> de MediatR)
│   ├── DTOs/             # LoginResponse (+ DTOs legados)
│   ├── Features/
│   │   ├── Auth/Commands/      # LoginCommand, RegisterCommand (+ handlers)
│   │   ├── Products/
│   │   │   ├── Commands/       # CreateProductCommand, UpdateProductCommand, DeleteProductCommand
│   │   │   └── Queries/        # GetAllProductsQuery, GetProductByIdQuery, SearchProductsQuery
│   │   ├── Categories/Queries/ # GetAllCategoriesQuery
│   │   └── Orders/
│   │       ├── Commands/       # CreateOrderCommand (+ handler)
│   │       └── Queries/        # GetOrderByIdQuery, GetOrdersByUserQuery
│   ├── Interfaces/       # IProductRepository, IOrderRepository, IUserRepository, ICategoryRepository, ITokenService, IPasswordHasher
│   └── Responses/        # ProductResponse, CategoryResponse, OrderResponse, OrderItemResponse
├── MiApp.Infrastructure/
│   ├── Migrations/
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/   # Fluent API para cada entidad
│   ├── Repositories/         # ProductRepository, OrderRepository, UserRepository, CategoryRepository
│   ├── Services/             # JwtTokenService, BCryptPasswordHasher
│   ├── DataSeeder.cs
│   └── InfrastructureServiceExtensions.cs
└── MiApp.WebApi/
    ├── Controllers/      # AuthController, ProductsController, OrdersController, CategoriesController
    └── Program.cs
tests/
└── MiApp.Tests/
    ├── Products/         # GetAllProductsQueryHandlerTests, GetProductByIdQueryHandlerTests, CreateProductCommandHandlerTests
    └── Orders/           # CreateOrderCommandHandlerTests
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

### Categories

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| GET | `/api/categories` | Listar todas las categorías | No |

### Orders

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| GET | `/api/orders/{id}` | Obtener orden con items | User |
| GET | `/api/orders/user/{userId}` | Órdenes de un usuario | User |
| POST | `/api/orders` | Crear orden | User |

---

## Autenticación

1. Hacer `POST /api/auth/login` con las credenciales.
2. La respuesta incluye `userId` y `token`:
```json
{
  "userId": "b1b2b3b4-0000-0000-0000-000000000001",
  "token": "eyJhbG..."
}
```
3. Copiar el `token` y en Swagger: click en **Authorize** → ingresar `Bearer <token>`.
4. Usar el `userId` para crear órdenes en `POST /api/orders`.

### Ejemplo de creación de orden

```json
POST /api/orders
{
  "userId": "b1b2b3b4-0000-0000-0000-000000000001",
  "items": [
    { "productId": "aaaaaaaa-0000-0000-0000-000000000001", "quantity": 2 },
    { "productId": "aaaaaaaa-0000-0000-0000-000000000002", "quantity": 1 }
  ]
}
```

---

## Tests

El proyecto incluye tests unitarios con **xUnit + Moq** que cubren los handlers de CQRS:

```bash
dotnet test tests/MiApp.Tests/MiApp.Tests.csproj
```

| Archivo | Tests |
|---|---|
| `GetAllProductsQueryHandlerTests` | Retorna todos, lista vacía, mapeo correcto |
| `GetProductByIdQueryHandlerTests` | Encontrado, no encontrado, Id correcto al repo |
| `CreateProductCommandHandlerTests` | Datos correctos, llama AddAsync, validaciones |
| `CreateOrderCommandHandlerTests` | Orden con total, producto no encontrado, stock insuficiente |

Los repositorios se mockean con **Moq** — los tests no dependen de base de datos.

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
