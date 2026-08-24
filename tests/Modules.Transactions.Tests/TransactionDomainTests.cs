using ExchangeTracing.Modules.Transactions.Domain;
using FluentAssertions;

namespace ExchangeTracing.Modules.Transactions.Tests;

public class TransactionDomainTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Create_rejects_non_positive_quantity(decimal quantity)
    {
        var act = () => Transaction.Create(Guid.NewGuid(), Guid.NewGuid(), TransactionType.Buy, quantity, 300m, DateTime.UtcNow);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Create_rejects_non_positive_price(decimal price)
    {
        var act = () => Transaction.Create(Guid.NewGuid(), Guid.NewGuid(), TransactionType.Buy, 10m, price, DateTime.UtcNow);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_sets_values_for_valid_input()
    {
        var userId = Guid.NewGuid();
        var assetId = Guid.NewGuid();

        var tx = Transaction.Create(userId, assetId, TransactionType.Buy, 100m, 300m, DateTime.UtcNow);

        tx.Id.Should().NotBe(Guid.Empty);
        tx.UserId.Should().Be(userId);
        tx.AssetId.Should().Be(assetId);
        tx.Quantity.Should().Be(100m);
        tx.Price.Should().Be(300m);
    }
}
