using ExchangeTracing.BuildingBlocks.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Users.Infrastructure;

/// <summary>
/// Users module's implementation of the cross-module <see cref="IUserExistence"/> contract.
/// Other modules depend only on the interface, never on this type or the Users DbContext.
/// </summary>
internal sealed class UserExistence(UsersDbContext context) : IUserExistence
{
    public Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
        => context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
}
