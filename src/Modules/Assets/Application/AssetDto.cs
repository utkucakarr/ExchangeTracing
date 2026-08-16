namespace ExchangeTracing.Modules.Assets.Application;

public sealed record AssetDto(
    Guid Id,
    string Symbol,
    string Name,
    string Exchange,
    string Currency,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
