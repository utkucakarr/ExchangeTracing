using MediatR;

namespace ExchangeTracing.Modules.Transactions.Application.ListTransactions;

public sealed record ListTransactionsQuery(Guid UserId) : IRequest<IReadOnlyList<TransactionDto>>;
