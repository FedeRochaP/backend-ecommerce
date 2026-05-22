using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;

namespace MiApp.Application.UseCases;

/// <summary>
/// QUERY: Retorna todos los productos disponibles.
/// No modifica estado — solo consulta.
/// </summary>
public class GetAllProductsUseCase
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> ExecuteAsync(CancellationToken ct = default)
        => await _productRepository.GetAllAsync(ct);
}
