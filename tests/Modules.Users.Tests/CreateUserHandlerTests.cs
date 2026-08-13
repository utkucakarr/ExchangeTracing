using ExchangeTracing.Modules.Users.Application;
using ExchangeTracing.Modules.Users.Application.CreateUser;
using ExchangeTracing.Modules.Users.Domain;
using FluentAssertions;
using Moq;

namespace ExchangeTracing.Modules.Users.Tests;

public class CreateUserHandlerTests
{
    private readonly Mock<IUserRepository> _users = new();

    private CreateUserHandler CreateSut() => new(_users.Object);

    [Fact]
    public async Task Creates_user_and_returns_dto_with_normalized_email()
    {
        _users.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreateUserCommand("  John.Doe@Example.COM ", "  John ", " Doe ");

        var result = await CreateSut().Handle(command, CancellationToken.None);

        result.Email.Should().Be("john.doe@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.IsActive.Should().BeTrue();
        result.Id.Should().NotBe(Guid.Empty);

        _users.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == "john.doe@example.com"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Throws_when_email_already_in_use()
    {
        _users.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreateUserCommand("taken@example.com", "Jane", "Doe");

        var act = () => CreateSut().Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<EmailAlreadyInUseException>();
        _users.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
