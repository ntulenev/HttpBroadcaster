namespace Abstractions;

/// <summary>
/// Represents a transactional unit of work that manages multiple outbox writers.
/// Ensures atomic writes across all target environments.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// Gets the collection of registered outbox writers for each environment.
    /// </summary>
    IReadOnlyCollection<IOutboxWriter> Writers { get; }

    /// <summary>
    /// Saves all pending changes and commits the transaction.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous commit operation.</returns>
    Task CommitAsync(CancellationToken cancellationToken);
}
