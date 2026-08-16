using MediatR;

namespace ExchangeTracing.Modules.Assets.Application.GetAsset;

public sealed record GetAssetQuery(Guid Id) : IRequest<AssetDto?>;
