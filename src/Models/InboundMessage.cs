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
    private InboundMessage(MessageId id, MessagePayload payload)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(payload);

        Id = id;
        Payload = payload;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InboundEvent"/> class.
    /// </summary>
    /// <param name="payload">The JSON payload.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="type"/> or <paramref name="payload"/> is null, empty, or whitespace.
    /// </exception>
    public static InboundMessage CreateNew(string payload)
    {
        var messagePayload = new MessagePayload(payload);
        var id = new MessageId(Guid.NewGuid());
        return new InboundMessage(id, messagePayload);
    }
}
