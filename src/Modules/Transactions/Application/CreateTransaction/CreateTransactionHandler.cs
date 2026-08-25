using ExchangeTracing.BuildingBlocks.Contracts;
using ExchangeTracing.Modules.Transactions.Domain;
using MediatR;

namespace ExchangeTracing.Modules.Transactions.Application.CreateTransaction;

public sealed class CreateTransactionHandler(
    ITransactionRepository transactions,
    IUserExistence users,
    IAssetExistence assets)
    : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // Cross-module checks via shared contracts (Users/Assets modules provide the implementations).
        if (!await users.ExistsAsync(request.UserId, cancellationToken))
        {
            throw new UserNotFoundException(request.UserId);
        }

        if (!await assets.ExistsAsync(request.AssetId, cancellationToken))
        {
            throw new AssetNotFoundException(request.AssetId);
        }

        // A sell cannot exceed the current position, derived from transaction history.
        if (request.Type == TransactionType.Sell)
        {
            var position = await transactions.GetNetQuantityAsync(request.UserId, request.AssetId, cancellationToken);
            if (request.Quantity > position)
            {
                throw new InsufficientPositionException(position, request.Quantity);
            }
        }

        var transaction = Transaction.Create(
            request.UserId,
            request.AssetId,
            request.Type,
            request.Quantity,
            request.Price,
            request.ExecutedAt);

        await transactions.AddAsync(transaction, cancellationToken);

        return transaction.ToDto();
    }
}
