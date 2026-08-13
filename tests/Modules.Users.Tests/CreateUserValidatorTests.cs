using ExchangeTracing.Modules.Users.Application.CreateUser;
using FluentAssertions;

namespace ExchangeTracing.Modules.Users.Tests;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    [Fact]
    public void Valid_command_passes()
    {
        var result = _validator.Validate(new CreateUserCommand("john@example.com", "John", "Doe"));
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "John", "Doe")]          // empty email
    [InlineData("not-an-email", "John", "Doe")] // invalid email
    [InlineData("john@example.com", "", "Doe")] // empty first name
    [InlineData("john@example.com", "John", "")] // empty last name
    public void Invalid_command_fails(string email, string firstName, string lastName)
    {
        var result = _validator.Validate(new CreateUserCommand(email, firstName, lastName));
        result.IsValid.Should().BeFalse();
    }
}
