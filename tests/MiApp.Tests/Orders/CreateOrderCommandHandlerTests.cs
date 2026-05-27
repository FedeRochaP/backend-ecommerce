using MiApp.Application.Features.Orders.Commands;
using Xunit;
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;
using MiApp.Domain.Exceptions;
using Moq;

namespace MiApp.Tests.Orders;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _orderRepoMock   = new Mock<IOrderRepository>();
        _productRepoMock = new Mock<IProductRepository>();

        _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask);

        _handler = new CreateOrderCommandHandler(_orderRepoMock.Object, _productRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsOrderResponse_WhenProductsExist()
    {
        // Arrange
        var userId    = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product   = new Product(productId, "Laptop", "Gamer", 1000m, 10, Guid.NewGuid());

        _productRepoMock.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

        var command = new CreateOrderCommand(userId, [new OrderLine(productId, 2)]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(2000m,  result.Total);
        Assert.Single(result.Items);
        Assert.Equal(productId, result.Items[0].ProductId);
        Assert.Equal(2,         result.Items[0].Quantity);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenProductNotFound()
    {
        // Arrange
        _productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Product?)null);

        var command = new CreateOrderCommand(Guid.NewGuid(), [new OrderLine(Guid.NewGuid(), 1)]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Throws_WhenInsufficientStock()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product   = new Product(productId, "Laptop", "Gamer", 1000m, 1, Guid.NewGuid()); // stock = 1

        _productRepoMock.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

        var command = new CreateOrderCommand(Guid.NewGuid(), [new OrderLine(productId, 5)]); // pide 5

        // Act & Assert
        await Assert.ThrowsAsync<InsufficientStockException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CallsAddAsync_Once_WhenSuccessful()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product   = new Product(productId, "Monitor", "4K", 800m, 5, Guid.NewGuid());

        _productRepoMock.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

        var command = new CreateOrderCommand(Guid.NewGuid(), [new OrderLine(productId, 1)]);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
