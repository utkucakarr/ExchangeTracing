using MediatR;

namespace ExchangeTracing.Modules.Transactions.Application.GetTransaction;

public sealed class GetTransactionHandler(ITransactionRepository transactions)
    : IRequestHandler<GetTransactionQuery, TransactionDto?>
{
    public async Task<TransactionDto?> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        var transaction = await transactions.GetByIdAsync(request.Id, cancellationToken);
        return transaction?.ToDto();
    }
}
