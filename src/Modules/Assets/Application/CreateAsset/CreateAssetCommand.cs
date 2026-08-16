using MediatR;

namespace ExchangeTracing.Modules.Assets.Application.CreateAsset;

public sealed record CreateAssetCommand(string Symbol, string Name) : IRequest<AssetDto>;
