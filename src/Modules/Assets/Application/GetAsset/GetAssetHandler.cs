using MediatR;

namespace ExchangeTracing.Modules.Assets.Application.GetAsset;

public sealed class GetAssetHandler(IAssetRepository assets)
    : IRequestHandler<GetAssetQuery, AssetDto?>
{
    public async Task<AssetDto?> Handle(GetAssetQuery request, CancellationToken cancellationToken)
    {
        var asset = await assets.GetByIdAsync(request.Id, cancellationToken);
        return asset?.ToDto();
    }
}
