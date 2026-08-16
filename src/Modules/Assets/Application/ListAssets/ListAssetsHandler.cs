using MediatR;

namespace ExchangeTracing.Modules.Assets.Application.ListAssets;

public sealed class ListAssetsHandler(IAssetRepository assets)
    : IRequestHandler<ListAssetsQuery, IReadOnlyList<AssetDto>>
{
    public async Task<IReadOnlyList<AssetDto>> Handle(ListAssetsQuery request, CancellationToken cancellationToken)
    {
        var items = await assets.ListAsync(cancellationToken);
        return items.Select(a => a.ToDto()).ToList();
    }
}
