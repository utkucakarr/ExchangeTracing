using ExchangeTracing.BuildingBlocks.Exceptions;

namespace ExchangeTracing.Modules.Transactions.Application;

public sealed class UserNotFoundException(Guid userId)
    : NotFoundException($"User '{userId}' does not exist.");

public sealed class AssetNotFoundException(Guid assetId)
    : NotFoundException($"Asset '{assetId}' does not exist.");

public sealed class InsufficientPositionException(decimal available, decimal requested)
    : ConflictException($"Cannot sell {requested}: only {available} available.");
