namespace Models;

/// <summary>
/// Represents a strongly-typed unique identifier for a message.
/// </summary>
/// <param name="Value">The underlying <see cref="Guid"/> value of the message ID.</param>
public sealed record MessageId(Guid Value);
