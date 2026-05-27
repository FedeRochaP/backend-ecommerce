using MiApp.Application.Features.Products.Queries;
using Xunit;
using MiApp.Application.Interfaces;
using MiApp.Application.Responses;
using MiApp.Domain.Entities;
using Moq;

namespace MiApp.Tests.Products;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _handler = new GetAllProductsQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllProducts_WhenRepositoryHasData()
    {
        // Arrange
        var catId = Guid.NewGuid();
        var products = new List<Product>
        {
            new(Guid.NewGuid(), "Laptop",  "Laptop gamer",  1500m, 10, catId),
            new(Guid.NewGuid(), "Monitor", "Monitor 4K",     800m,  5, catId)
        };
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, p => p.Name == "Laptop");
        Assert.Contains(list, p => p.Name == "Monitor");
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoProductsExist()
    {
        // Arrange
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<Product>());

        // Act
        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_MapsToProductResponse_Correctly()
    {
        // Arrange
        var id     = Guid.NewGuid();
        var catId  = Guid.NewGuid();
        var product = new Product(id, "Teclado", "Mecánico", 200m, 15, catId);
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<Product> { product });

        // Act
        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        // Assert
        var response = Assert.Single(result);
        Assert.IsType<ProductResponse>(response);
        Assert.Equal(id,       response.Id);
        Assert.Equal("Teclado", response.Name);
        Assert.Equal(200m,     response.Price);
        Assert.Equal(15,       response.Stock);
        Assert.Equal(catId,    response.CategoryId);
    }
}
