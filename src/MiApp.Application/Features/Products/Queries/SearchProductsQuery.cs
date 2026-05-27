using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;
using MiApp.Application.Responses;

namespace MiApp.Application.Features.Products.Queries;

public record SearchProductsQuery(string Name) : IQuery<IEnumerable<ProductResponse>>;

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, IEnumerable<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public SearchProductsQueryHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<IEnumerable<ProductResponse>> Handle(SearchProductsQuery query, CancellationToken ct)
    {
        var products = await _productRepository.SearchByNameAsync(query.Name, ct);
        return products.Select(p => new ProductResponse(
            p.Id, p.Name, p.Description, p.Price, p.Stock, p.CategoryId, p.CreatedAt));
    }
}
