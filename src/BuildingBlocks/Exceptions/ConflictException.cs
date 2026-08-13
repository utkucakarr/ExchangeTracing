namespace ExchangeTracing.BuildingBlocks.Exceptions;

/// <summary>
/// Thrown when a request conflicts with the current state (e.g. a uniqueness rule).
/// Mapped to HTTP 409 by the API's global exception handler.
/// </summary>
public class ConflictException(string message) : Exception(message);
