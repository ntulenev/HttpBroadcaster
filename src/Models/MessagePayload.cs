namespace Models;

/// <summary>
/// Represents the payload of a message as a validated non-empty string.
/// </summary>
public sealed class MessagePayload
{
    /// <summary>
    /// Gets the raw string payload.
    /// </summary>
    public string Payload { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagePayload"/> class.
    /// </summary>
    /// <param name="payload">The string content of the message payload.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="payload"/> is null, empty, or consists only of whitespace.
    /// </exception>
    public MessagePayload(string payload)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);
        Payload = payload;
    }
}
