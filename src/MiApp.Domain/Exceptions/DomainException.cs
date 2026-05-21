namespace MiApp.Domain.Exceptions;

/// <summary>
/// Clase base para todas las excepciones de dominio.
/// Se lanza cuando se viola una regla de negocio.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}
