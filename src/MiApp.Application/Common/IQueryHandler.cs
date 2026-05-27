namespace MiApp.Application.Common;

/// <summary>Contrato que debe implementar todo handler de consulta CQRS.</summary>
public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken ct = default);
}
