namespace ExchangeTracing.Modules.Users.Domain;

/// <summary>
/// A user of the system. Created through <see cref="Create"/> so it is always in a
/// valid state; setters are private so state changes go through domain behavior.
/// </summary>
public sealed class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private User()
    {
        // Required by EF Core.
    }

    private User(Guid id, string email, string firstName, string lastName, DateTime timestamp)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        IsActive = true;
        CreatedAt = timestamp;
        UpdatedAt = timestamp;
    }

    public static User Create(string email, string firstName, string lastName)
    {
        var now = DateTime.UtcNow;
        return new User(
            Guid.NewGuid(),
            email.Trim().ToLowerInvariant(),
            firstName.Trim(),
            lastName.Trim(),
            now);
    }
}
