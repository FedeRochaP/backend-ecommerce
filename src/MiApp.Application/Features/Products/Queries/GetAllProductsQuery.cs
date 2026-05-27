using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;
using MiApp.Application.Responses;

namespace MiApp.Application.Features.Products.Queries;

public record GetAllProductsQuery : IQuery<IEnumerable<ProductResponse>>;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsQuery query, CancellationToken ct)
    {
        var products = await _productRepository.GetAllAsync(ct);
        return products.Select(p => new ProductResponse(
            p.Id, p.Name, p.Description, p.Price, p.Stock, p.CategoryId, p.CreatedAt));
    }
}
