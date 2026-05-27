using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;
using MiApp.Application.Responses;

namespace MiApp.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse?>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<ProductResponse?> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(query.Id, ct);
        if (product is null) return null;

        return new ProductResponse(
            product.Id, product.Name, product.Description, product.Price, product.Stock, product.CategoryId, product.CreatedAt);
    }
}
