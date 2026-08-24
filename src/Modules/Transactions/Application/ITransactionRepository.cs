using ExchangeTracing.Modules.Transactions.Domain;

namespace ExchangeTracing.Modules.Transactions.Application;

/// <summary>
/// Persistence boundary for transactions. Focused (not generic) so the Application layer
/// stays free of EF Core and can be unit tested with a mock.
/// </summary>
public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);

    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Transaction>> ListByUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Current net position for a user/asset: sum of BUY quantities minus sum of SELL quantities.
    /// Derived from transaction history (transactions are the source of truth).
    /// </summary>
    Task<decimal> GetNetQuantityAsync(Guid userId, Guid assetId, CancellationToken cancellationToken);
}
