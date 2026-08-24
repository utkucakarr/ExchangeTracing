namespace ExchangeTracing.BuildingBlocks.Contracts;

/// <summary>
/// Cross-module contract: lets other modules check whether a user exists without
/// referencing the Users module directly. The Users module provides the implementation;
/// the composition root wires it up. Enables synchronous, in-process module communication.
/// </summary>
public interface IUserExistence
{
    Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken);
}
