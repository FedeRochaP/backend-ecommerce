using MiApp.Application.Features.Products.Queries;
using Xunit;
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;
using Moq;

namespace MiApp.Tests.Products;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _handler = new GetProductByIdQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsProductResponse_WhenProductExists()
    {
        // Arrange
        var id      = Guid.NewGuid();
        var catId   = Guid.NewGuid();
        var product = new Product(id, "Laptop", "Gamer", 1500m, 10, catId);

        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(new GetProductByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id,       result.Id);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal(1500m,    result.Price);
        Assert.Equal(10,       result.Stock);
        Assert.Equal(catId,    result.CategoryId);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenProductDoesNotExist()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Product?)null);

        // Act
        var result = await _handler.Handle(new GetProductByIdQuery(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_CallsRepository_WithCorrectId()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Product?)null);

        // Act
        await _handler.Handle(new GetProductByIdQuery(id), CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
