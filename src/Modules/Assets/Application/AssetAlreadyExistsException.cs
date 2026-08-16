using ExchangeTracing.BuildingBlocks.Exceptions;

namespace ExchangeTracing.Modules.Assets.Application;

public sealed class AssetAlreadyExistsException(string exchange, string symbol)
    : ConflictException($"An asset '{symbol}' already exists on exchange '{exchange}'.");
