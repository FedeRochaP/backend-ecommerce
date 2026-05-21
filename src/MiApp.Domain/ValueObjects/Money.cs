namespace MiApp.Domain.ValueObjects;

/// <summary>
/// Value Object que representa un monto monetario con su moneda.
/// Es inmutable: dos instancias con el mismo Amount y Currency son iguales.
/// </summary>
public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "ARS")
    {
        if (amount < 0)
            throw new ArgumentException("El monto no puede ser negativo.", nameof(amount));

        Amount = amount;
        Currency = currency;
    }

    public static Money Zero => new(0);

    public Money Add(Money other) => new(Amount + other.Amount, Currency);

    public Money Multiply(int factor) => new(Amount * factor, Currency);

    public override string ToString() => $"{Currency} {Amount:F2}";
}
