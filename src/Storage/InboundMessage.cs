namespace Storage;

/// <summary>
/// Represents an inbound message DB model.
/// </summary>
public class InboundMessage
{

    /// <summary>
    /// Gets the unique identifier of the message.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Gets the message payload.
    /// </summary>
    public required string Payload { get; init; }

    /// <summary>
    /// Gets the UTC timestamp when the message was created.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }
}
