using ExchangeTracing.Modules.Assets.Domain;

namespace ExchangeTracing.Modules.Assets.Application;

/// <summary>
/// Persistence boundary for assets. Focused (not generic) so the Application layer stays
/// free of EF Core and can be unit tested with a mock.
/// </summary>
public interface IAssetRepository
{
    Task<bool> ExistsByExchangeAndSymbolAsync(string exchange, string symbol, CancellationToken cancellationToken);

    Task AddAsync(Asset asset, CancellationToken cancellationToken);

    Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Asset>> ListAsync(CancellationToken cancellationToken);
}
