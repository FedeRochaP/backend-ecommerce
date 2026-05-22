using MiApp.Domain.Entities;

namespace MiApp.Application.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default);
}
