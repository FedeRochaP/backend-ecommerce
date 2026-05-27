using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;

namespace MiApp.Application.Features.Products.Commands;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, int Stock)
    : ICommand<bool>;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<bool> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, ct);
        if (product is null) return false;

        product.Update(command.Name, command.Description, command.Price, command.Stock);
        await _productRepository.UpdateAsync(product, ct);
        return true;
    }
}
