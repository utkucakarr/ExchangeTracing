namespace ExchangeTracing.Modules.Transactions.Domain;

/// <summary>
/// A completed buy or sell operation — the source of truth for portfolio state.
/// Created through <see cref="Create"/> so it is always valid; setters are private.
/// Monetary/quantity values use decimal (never float/double).
/// </summary>
public sealed class Transaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AssetId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal Price { get; private set; }
    public DateTime ExecutedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Transaction()
    {
        // Required by EF Core.
    }

    private Transaction(
        Guid id,
        Guid userId,
        Guid assetId,
        TransactionType type,
        decimal quantity,
        decimal price,
        DateTime executedAt,
        DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        AssetId = assetId;
        Type = type;
        Quantity = quantity;
        Price = price;
        ExecutedAt = executedAt;
        CreatedAt = createdAt;
    }

    public static Transaction Create(
        Guid userId,
        Guid assetId,
        TransactionType type,
        decimal quantity,
        decimal price,
        DateTime executedAt)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        if (price <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero.");
        }

        return new Transaction(
            Guid.NewGuid(),
            userId,
            assetId,
            type,
            quantity,
            price,
            executedAt,
            DateTime.UtcNow);
    }
}
