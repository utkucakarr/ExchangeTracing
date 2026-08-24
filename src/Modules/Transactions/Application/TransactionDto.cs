using ExchangeTracing.Modules.Transactions.Domain;

namespace ExchangeTracing.Modules.Transactions.Application;

public sealed record TransactionDto(
    Guid Id,
    Guid UserId,
    Guid AssetId,
    TransactionType Type,
    decimal Quantity,
    decimal Price,
    DateTime ExecutedAt,
    DateTime CreatedAt);
