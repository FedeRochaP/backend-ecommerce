using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;
using MiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiApp.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default)
        => await _context.Categories.AsNoTracking().ToListAsync(ct);
}
