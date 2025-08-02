using Abstractions;

using Microsoft.Extensions.Logging;

using Models;

namespace Logic;

/// <summary>
/// Handles broadcasting inbound messages to all configured outbox environments.
/// </summary>
public sealed class BroadcastingHandler : IBroadcastingHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BroadcastingHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for structured logging.</param>
    /// <param name="unitOfWork">The unit of work for writing messages to outbox tables.</param>
    public BroadcastingHandler(ILogger<BroadcastingHandler> logger, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(unitOfWork);

        _logger = logger;
        _uow = unitOfWork;
    }

    /// <summary>
    /// Broadcasts the specified message to all environments by writing it to their corresponding outbox tables.
    /// </summary>
    /// <param name="message">The inbound message to broadcast.</param>
    /// <param name="ct">A cancellation token to observe while performing the operation.</param>
    /// <returns>A task that represents the asynchronous broadcast operation.</returns>
    public async Task BroadcastAsync(InboundMessage message, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);
        ct.ThrowIfCancellationRequested();

        using var _ = _logger.BeginScope("Message {MessageId}", message.Id);
        _logger.LogInformation("Begin broadcasting message");

        foreach (var writer in _uow.Writers)
        {
            _logger.LogInformation("Write message for {Environment}", writer.Environment);
            await writer.WriteAsync(message, ct);
        }

        await _uow.CommitAsync(ct);
        _logger.LogInformation("Message broadcasted successfully");
    }

    private readonly IUnitOfWork _uow;
    private readonly ILogger _logger;
}