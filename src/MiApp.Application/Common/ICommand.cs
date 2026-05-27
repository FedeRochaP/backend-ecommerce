using MediatR;

namespace MiApp.Application.Common;

/// <summary>Marca un objeto como un comando CQRS que retorna TResult. Extiende IRequest para que MediatR lo dispatche automáticamente.</summary>
public interface ICommand<TResult> : IRequest<TResult> { }
