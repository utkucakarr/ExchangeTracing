using ExchangeTracing.BuildingBlocks.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Assets.Infrastructure;

/// <summary>
/// Assets module's implementation of the cross-module <see cref="IAssetExistence"/> contract.
/// Other modules depend only on the interface, never on this type or the Assets DbContext.
/// </summary>
internal sealed class AssetExistence(AssetsDbContext context) : IAssetExistence
{
    public Task<bool> ExistsAsync(Guid assetId, CancellationToken cancellationToken)
        => context.Assets.AnyAsync(a => a.Id == assetId, cancellationToken);
}
