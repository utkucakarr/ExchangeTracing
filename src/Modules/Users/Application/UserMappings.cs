using ExchangeTracing.Modules.Users.Domain;

namespace ExchangeTracing.Modules.Users.Application;

internal static class UserMappings
{
    public static UserDto ToDto(this User user) => new(
        user.Id,
        user.Email,
        user.FirstName,
        user.LastName,
        user.IsActive,
        user.CreatedAt,
        user.UpdatedAt);
}
