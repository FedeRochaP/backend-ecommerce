using MediatR;

namespace MiApp.Application.Common;

/// <summary>Marca un objeto como una consulta CQRS que retorna TResult. Extiende IRequest para que MediatR lo dispatche automáticamente.</summary>
public interface IQuery<TResult> : IRequest<TResult> { }
