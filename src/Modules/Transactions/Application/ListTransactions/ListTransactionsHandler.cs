using MediatR;

namespace ExchangeTracing.Modules.Transactions.Application.ListTransactions;

public sealed class ListTransactionsHandler(ITransactionRepository transactions)
    : IRequestHandler<ListTransactionsQuery, IReadOnlyList<TransactionDto>>
{
    public async Task<IReadOnlyList<TransactionDto>> Handle(ListTransactionsQuery request, CancellationToken cancellationToken)
    {
        var items = await transactions.ListByUserAsync(request.UserId, cancellationToken);
        return items.Select(t => t.ToDto()).ToList();
    }
}
