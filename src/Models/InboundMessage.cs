namespace Models;

/// <summary>
/// Represents an inbound message intended for dispatching to one or more outbox environments.
/// </summary>
public sealed class InboundMessage
{
    /// <summary>
    /// Gets the unique identifier of the message.
    /// </summary>
    public MessageId Id { get; }

    /// <summary>
    /// Gets the message payload.
    /// </summary>
    public MessagePayload Payload { get; }

    /// <summary>
    /// Gets the UTC timestamp when the message was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InboundMessage"/> class
    /// with the specified message ID and payload.
    /// </summary>
    /// <param name="id">The unique identifier of the message.</param>
    /// <param name="payload">The message payload.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="id"/> or <paramref name="payload"/> is null.</exception>
    public InboundMessage(MessageId id, MessagePayload payload)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(payload);

        Id = id;
        Payload = payload;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
