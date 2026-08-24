namespace ExchangeTracing.BuildingBlocks.Contracts;

/// <summary>
/// Cross-module contract: lets other modules check whether an asset exists without
/// referencing the Assets module directly. The Assets module provides the implementation;
/// the composition root wires it up. Enables synchronous, in-process module communication.
/// </summary>
public interface IAssetExistence
{
    Task<bool> ExistsAsync(Guid assetId, CancellationToken cancellationToken);
}
