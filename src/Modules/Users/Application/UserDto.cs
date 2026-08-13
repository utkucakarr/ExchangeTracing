namespace ExchangeTracing.Modules.Users.Application;

public sealed record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
