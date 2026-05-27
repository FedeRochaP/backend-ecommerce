namespace MiApp.Application.Common;

/// <summary>Contrato que debe implementar todo handler de comando CQRS.</summary>
public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken ct = default);
}
