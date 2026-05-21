namespace MiApp.Domain.Exceptions;

/// <summary>
/// Se lanza cuando se intenta reducir el stock de un producto
/// por encima de las unidades disponibles.
/// </summary>
public class InsufficientStockException : DomainException
{
    public InsufficientStockException(string productName, int requested, int available)
        : base($"Stock insuficiente para '{productName}'. Solicitado: {requested}, disponible: {available}.") { }
}
