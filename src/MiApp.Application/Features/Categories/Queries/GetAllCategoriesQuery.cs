using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;
using MiApp.Application.Responses;

namespace MiApp.Application.Features.Categories.Queries;

public record GetAllCategoriesQuery : IQuery<IEnumerable<CategoryResponse>>;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task<IEnumerable<CategoryResponse>> Handle(GetAllCategoriesQuery query, CancellationToken ct)
    {
        var categories = await _categoryRepository.GetAllAsync(ct);
        return categories.Select(c => new CategoryResponse(c.Id, c.Name));
    }
}
