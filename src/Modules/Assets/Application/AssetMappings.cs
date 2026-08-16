using ExchangeTracing.Modules.Assets.Domain;

namespace ExchangeTracing.Modules.Assets.Application;

internal static class AssetMappings
{
    public static AssetDto ToDto(this Asset asset) => new(
        asset.Id,
        asset.Symbol,
        asset.Name,
        asset.Exchange,
        asset.Currency,
        asset.IsActive,
        asset.CreatedAt,
        asset.UpdatedAt);
}
