using MediatR;

namespace ExchangeTracing.Modules.Users.Application.GetUser;

public sealed class GetUserHandler(IUserRepository users)
    : IRequestHandler<GetUserQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await users.GetByIdAsync(request.Id, cancellationToken);
        return user?.ToDto();
    }
}
