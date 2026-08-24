using ExchangeTracing.Modules.Transactions.Domain;
using MediatR;

namespace ExchangeTracing.Modules.Transactions.Application.CreateTransaction;

public sealed record CreateTransactionCommand(
    Guid UserId,
    Guid AssetId,
    TransactionType Type,
    decimal Quantity,
    decimal Price,
    DateTime ExecutedAt) : IRequest<TransactionDto>;
