using ExchangeTracing.Modules.Users.Application;
using ExchangeTracing.Modules.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Users.Infrastructure;

internal sealed class UserRepository(UsersDbContext context) : IUserRepository
{
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        => context.Users.AnyAsync(u => u.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
}
