using ExchangeTracing.Modules.Assets.Application.CreateAsset;
using FluentAssertions;

namespace ExchangeTracing.Modules.Assets.Tests;

public class CreateAssetValidatorTests
{
    private readonly CreateAssetValidator _validator = new();

    [Fact]
    public void Valid_command_passes()
    {
        var result = _validator.Validate(new CreateAssetCommand("THYAO", "Türk Hava Yolları"));
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Türk Hava Yolları")]  // empty symbol
    [InlineData("THYAO", "")]              // empty name
    public void Invalid_command_fails(string symbol, string name)
    {
        var result = _validator.Validate(new CreateAssetCommand(symbol, name));
        result.IsValid.Should().BeFalse();
    }
}
