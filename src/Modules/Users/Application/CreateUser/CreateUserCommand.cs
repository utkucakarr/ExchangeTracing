using MediatR;

namespace ExchangeTracing.Modules.Users.Application.CreateUser;

public sealed record CreateUserCommand(string Email, string FirstName, string LastName)
    : IRequest<UserDto>;
