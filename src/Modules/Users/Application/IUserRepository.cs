using ExchangeTracing.Modules.Users.Domain;

namespace ExchangeTracing.Modules.Users.Application;

/// <summary>
/// Persistence boundary for users. Focused (not generic) so the Application layer stays
/// free of EF Core and can be unit tested with a mock.
/// </summary>
public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);

    Task AddAsync(User user, CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
