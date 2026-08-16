using MediatR;

namespace ExchangeTracing.Modules.Assets.Application.ListAssets;

public sealed record ListAssetsQuery : IRequest<IReadOnlyList<AssetDto>>;
