using ExchangeTracing.Modules.Assets.Application;
using ExchangeTracing.Modules.Assets.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Assets.Infrastructure;

internal sealed class AssetRepository(AssetsDbContext context) : IAssetRepository
{
    public Task<bool> ExistsByExchangeAndSymbolAsync(string exchange, string symbol, CancellationToken cancellationToken)
        => context.Assets.AnyAsync(a => a.Exchange == exchange && a.Symbol == symbol, cancellationToken);

    public async Task AddAsync(Asset asset, CancellationToken cancellationToken)
    {
        await context.Assets.AddAsync(asset, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => context.Assets.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Asset>> ListAsync(CancellationToken cancellationToken)
        => await context.Assets
            .OrderBy(a => a.Symbol)
            .ToListAsync(cancellationToken);
}
