using MediatR;

namespace ExchangeTracing.Modules.Users.Application.GetUser;

public sealed record GetUserQuery(Guid Id) : IRequest<UserDto?>;
