using ExchangeTracing.Modules.Assets.Domain;
using MediatR;

namespace ExchangeTracing.Modules.Assets.Application.CreateAsset;

public sealed class CreateAssetHandler(IAssetRepository assets)
    : IRequestHandler<CreateAssetCommand, AssetDto>
{
    public async Task<AssetDto> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var symbol = request.Symbol.Trim().ToUpperInvariant();

        if (await assets.ExistsByExchangeAndSymbolAsync(Asset.DefaultExchange, symbol, cancellationToken))
        {
            throw new AssetAlreadyExistsException(Asset.DefaultExchange, symbol);
        }

        var asset = Asset.Create(request.Symbol, request.Name);
        await assets.AddAsync(asset, cancellationToken);

        return asset.ToDto();
    }
}
