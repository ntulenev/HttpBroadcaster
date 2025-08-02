using Models;

namespace Abstractions;

/// <summary>
/// Defines a contract for broadcasting an inbound message to all configured outbox environments.
/// </summary>
public interface IBroadcastingHandler
{
    /// <summary>
    /// Broadcasts the given inbound message by writing it to all configured outbox writers.
    /// </summary>
    /// <param name="message">The inbound message to broadcast.</param>
    /// <param name="ct">A cancellation token to observe while performing the operation.</param>
    /// <returns>A task representing the asynchronous broadcast operation.</returns>
    Task BroadcastAsync(InboundMessage message, CancellationToken ct);
}
