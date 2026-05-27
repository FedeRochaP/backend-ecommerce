using MiApp.Application.Features.Products.Commands;
using Xunit;
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;
using Moq;

namespace MiApp.Tests.Products;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        _handler = new CreateProductCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsProductResponse_WithCorrectData()
    {
        // Arrange
        var catId   = Guid.NewGuid();
        var command = new CreateProductCommand("Laptop", "Gamer 4K", 1500m, 10, catId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Laptop",   result.Name);
        Assert.Equal("Gamer 4K", result.Description);
        Assert.Equal(1500m,      result.Price);
        Assert.Equal(10,         result.Stock);
        Assert.Equal(catId,      result.CategoryId);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task Handle_CallsAddAsync_Once()
    {
        // Arrange
        var command = new CreateProductCommand("Mouse", "Inalámbrico", 50m, 20, Guid.NewGuid());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Throws_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateProductCommand("", "Desc", 100m, 5, Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Throws_WhenPriceIsNegative()
    {
        // Arrange
        var command = new CreateProductCommand("Teclado", "Mecánico", -10m, 5, Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
