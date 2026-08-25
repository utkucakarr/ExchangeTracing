using FluentValidation;

namespace ExchangeTracing.Modules.Transactions.Application.ListTransactions;

public sealed class ListTransactionsValidator : AbstractValidator<ListTransactionsQuery>
{
    public ListTransactionsValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
