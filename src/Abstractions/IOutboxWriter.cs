using Models;

namespace Abstractions;

/// <summary>
/// Defines a writer that persists messages to a specific environment's outbox table.
/// </summary>
public interface IOutboxWriter
{
    /// <summary>
    /// Gets the environment this writer is associated with.
    /// </summary>
    Models.Environment Environment { get; }

    /// <summary>
    /// Writes the specified inbound message to the outbox table.
    /// </summary>
    /// <param name="message">The message to write.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task WriteAsync(InboundMessage message, CancellationToken ct);
}
