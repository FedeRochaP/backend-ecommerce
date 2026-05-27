using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;
using MiApp.Application.Responses;
using MiApp.Domain.Entities;

namespace MiApp.Application.Features.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId) : ICommand<ProductResponse>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<ProductResponse> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var product = new Product(
            Guid.NewGuid(),
            command.Name,
            command.Description,
            command.Price,
            command.Stock,
            command.CategoryId);

        await _productRepository.AddAsync(product, ct);

        return new ProductResponse(
            product.Id, product.Name, product.Description, product.Price, product.Stock, product.CategoryId, product.CreatedAt);
    }
}
