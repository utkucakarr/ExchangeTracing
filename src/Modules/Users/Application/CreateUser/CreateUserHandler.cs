using ExchangeTracing.Modules.Users.Domain;
using MediatR;

namespace ExchangeTracing.Modules.Users.Application.CreateUser;

public sealed class CreateUserHandler(IUserRepository users)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await users.ExistsByEmailAsync(email, cancellationToken))
        {
            throw new EmailAlreadyInUseException(email);
        }

        var user = User.Create(request.Email, request.FirstName, request.LastName);
        await users.AddAsync(user, cancellationToken);

        return user.ToDto();
    }
}
