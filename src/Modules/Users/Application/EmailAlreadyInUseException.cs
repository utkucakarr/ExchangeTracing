using ExchangeTracing.BuildingBlocks.Exceptions;

namespace ExchangeTracing.Modules.Users.Application;

public sealed class EmailAlreadyInUseException(string email)
    : ConflictException($"A user with email '{email}' already exists.");
