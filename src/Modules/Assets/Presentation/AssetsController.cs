using ExchangeTracing.Modules.Assets.Application;
using ExchangeTracing.Modules.Assets.Application.CreateAsset;
using ExchangeTracing.Modules.Assets.Application.GetAsset;
using ExchangeTracing.Modules.Assets.Application.ListAssets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeTracing.Modules.Assets.Presentation;

[ApiController]
[Route("assets")]
public sealed class AssetsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AssetDto>> Create(
        CreateAssetCommand command,
        CancellationToken cancellationToken)
    {
        var asset = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AssetDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var asset = await sender.Send(new GetAssetQuery(id), cancellationToken);
        return asset is null ? NotFound() : Ok(asset);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssetDto>>> List(CancellationToken cancellationToken)
    {
        var assets = await sender.Send(new ListAssetsQuery(), cancellationToken);
        return Ok(assets);
    }
}
