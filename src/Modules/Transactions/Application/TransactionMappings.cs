using ExchangeTracing.Modules.Transactions.Domain;

namespace ExchangeTracing.Modules.Transactions.Application;

internal static class TransactionMappings
{
    public static TransactionDto ToDto(this Transaction transaction) => new(
        transaction.Id,
        transaction.UserId,
        transaction.AssetId,
        transaction.Type,
        transaction.Quantity,
        transaction.Price,
        transaction.ExecutedAt,
        transaction.CreatedAt);
}
