namespace ExchangeTracing.Modules.Assets.Domain;

/// <summary>
/// A tradable asset. Created through <see cref="Create"/> so it is always in a valid
/// state; setters are private so state changes go through domain behavior.
/// Scope is currently Borsa İstanbul only, so Exchange/Currency are fixed defaults.
/// </summary>
public sealed class Asset
{
    public const string DefaultExchange = "BIST";
    public const string DefaultCurrency = "TRY";

    public Guid Id { get; private set; }
    public string Symbol { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Exchange { get; private set; } = null!;
    public string Currency { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Asset()
    {
        // Required by EF Core.
    }

    private Asset(Guid id, string symbol, string name, string exchange, string currency, DateTime timestamp)
    {
        Id = id;
        Symbol = symbol;
        Name = name;
        Exchange = exchange;
        Currency = currency;
        IsActive = true;
        CreatedAt = timestamp;
        UpdatedAt = timestamp;
    }

    public static Asset Create(string symbol, string name)
    {
        var now = DateTime.UtcNow;
        return new Asset(
            Guid.NewGuid(),
            symbol.Trim().ToUpperInvariant(),
            name.Trim(),
            DefaultExchange,
            DefaultCurrency,
            now);
    }
}
