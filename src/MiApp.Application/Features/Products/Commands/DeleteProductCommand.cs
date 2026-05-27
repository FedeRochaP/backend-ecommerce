using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;

namespace MiApp.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid Id) : ICommand<bool>;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<bool> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, ct);
        if (product is null) return false;

        await _productRepository.DeleteAsync(product, ct);
        return true;
    }
}
