namespace ExchangeTracing.BuildingBlocks.Exceptions;

/// <summary>
/// Thrown when a referenced resource does not exist.
/// Mapped to HTTP 404 by the API's global exception handler.
/// </summary>
public class NotFoundException(string message) : Exception(message);
