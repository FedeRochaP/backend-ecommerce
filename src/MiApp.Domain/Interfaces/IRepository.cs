namespace MiApp.Domain.Interfaces;

/// <summary>
/// Contrato base genérico para todos los repositorios.
/// Define las operaciones mínimas que cualquier repositorio debe exponer.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
}
