using ExchangeTracing.Modules.Transactions.Application;
using ExchangeTracing.Modules.Transactions.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Transactions.Infrastructure;

internal sealed class TransactionRepository(TransactionsDbContext context) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        await context.Transactions.AddAsync(transaction, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => context.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Transaction>> ListByUserAsync(Guid userId, CancellationToken cancellationToken)
        => await context.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.ExecutedAt)
            .ToListAsync(cancellationToken);

    public async Task<decimal> GetNetQuantityAsync(Guid userId, Guid assetId, CancellationToken cancellationToken)
    {
        // Net position derived in the database: sum of BUY minus sum of SELL quantities.
        var query = context.Transactions.Where(t => t.UserId == userId && t.AssetId == assetId);

        var bought = await query
            .Where(t => t.Type == TransactionType.Buy)
            .SumAsync(t => (decimal?)t.Quantity, cancellationToken) ?? 0m;

        var sold = await query
            .Where(t => t.Type == TransactionType.Sell)
            .SumAsync(t => (decimal?)t.Quantity, cancellationToken) ?? 0m;

        return bought - sold;
    }
}
