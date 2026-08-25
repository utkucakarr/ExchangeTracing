using MediatR;

namespace ExchangeTracing.Modules.Transactions.Application.GetTransaction;

public sealed record GetTransactionQuery(Guid Id) : IRequest<TransactionDto?>;
