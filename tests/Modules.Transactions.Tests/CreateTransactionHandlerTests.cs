using ExchangeTracing.BuildingBlocks.Contracts;
using ExchangeTracing.Modules.Transactions.Application;
using ExchangeTracing.Modules.Transactions.Application.CreateTransaction;
using ExchangeTracing.Modules.Transactions.Domain;
using FluentAssertions;
using Moq;

namespace ExchangeTracing.Modules.Transactions.Tests;

public class CreateTransactionHandlerTests
{
    private readonly Mock<ITransactionRepository> _repo = new();
    private readonly Mock<IUserExistence> _users = new();
    private readonly Mock<IAssetExistence> _assets = new();

    public CreateTransactionHandlerTests()
    {
        // Default: user and asset exist. Individual tests override as needed.
        _users.Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _assets.Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
    }

    private CreateTransactionHandler CreateSut() => new(_repo.Object, _users.Object, _assets.Object);

    private static CreateTransactionCommand Command(TransactionType type, decimal quantity) =>
        new(Guid.NewGuid(), Guid.NewGuid(), type, quantity, 300m, DateTime.UtcNow);

    [Fact]
    public async Task Creates_buy_transaction_when_user_and_asset_exist()
    {
        var result = await CreateSut().Handle(Command(TransactionType.Buy, 100m), CancellationToken.None);

        result.Type.Should().Be(TransactionType.Buy);
        result.Quantity.Should().Be(100m);
        _repo.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Throws_when_user_does_not_exist()
    {
        _users.Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = () => CreateSut().Handle(Command(TransactionType.Buy, 100m), CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>();
        _repo.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Throws_when_asset_does_not_exist()
    {
        _assets.Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = () => CreateSut().Handle(Command(TransactionType.Buy, 100m), CancellationToken.None);

        await act.Should().ThrowAsync<AssetNotFoundException>();
        _repo.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Throws_when_selling_more_than_current_position()
    {
        _repo.Setup(r => r.GetNetQuantityAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(100m);

        var act = () => CreateSut().Handle(Command(TransactionType.Sell, 150m), CancellationToken.None);

        await act.Should().ThrowAsync<InsufficientPositionException>();
        _repo.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Allows_sell_within_current_position()
    {
        _repo.Setup(r => r.GetNetQuantityAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(100m);

        var result = await CreateSut().Handle(Command(TransactionType.Sell, 40m), CancellationToken.None);

        result.Type.Should().Be(TransactionType.Sell);
        _repo.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
