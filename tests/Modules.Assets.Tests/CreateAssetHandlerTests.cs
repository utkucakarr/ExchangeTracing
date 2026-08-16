using ExchangeTracing.Modules.Assets.Application;
using ExchangeTracing.Modules.Assets.Application.CreateAsset;
using ExchangeTracing.Modules.Assets.Domain;
using FluentAssertions;
using Moq;

namespace ExchangeTracing.Modules.Assets.Tests;

public class CreateAssetHandlerTests
{
    private readonly Mock<IAssetRepository> _assets = new();

    private CreateAssetHandler CreateSut() => new(_assets.Object);

    [Fact]
    public async Task Creates_asset_with_normalized_symbol_and_fixed_bist_try()
    {
        _assets.Setup(r => r.ExistsByExchangeAndSymbolAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreateAssetCommand("  thyao ", "  Türk Hava Yolları ");

        var result = await CreateSut().Handle(command, CancellationToken.None);

        result.Symbol.Should().Be("THYAO");
        result.Name.Should().Be("Türk Hava Yolları");
        result.Exchange.Should().Be("BIST");
        result.Currency.Should().Be("TRY");
        result.IsActive.Should().BeTrue();
        result.Id.Should().NotBe(Guid.Empty);

        _assets.Verify(r => r.AddAsync(It.Is<Asset>(a => a.Symbol == "THYAO"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Throws_when_asset_already_exists_on_exchange()
    {
        _assets.Setup(r => r.ExistsByExchangeAndSymbolAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreateAssetCommand("THYAO", "duplicate");

        var act = () => CreateSut().Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<AssetAlreadyExistsException>();
        _assets.Verify(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
