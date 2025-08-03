using System.Collections.Frozen;
using System.Data;

using Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Storage.Configuration;

namespace Storage;

/// <summary>
/// Implements the unit of work pattern for writing events to multiple outbox tables atomically.
/// Each outbox is tied to a specific environment, and all writes are wrapped in a single transaction.
/// </summary>
public sealed class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// Begins a transaction and creates outbox writers for all configured environments.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="config">The configuration containing the list of environments.</param>
    /// <param name="writeFactory">A factory that creates an <see cref="IOutboxWriter"/> for a given environment.</param>
    /// <param name="logger">The logger for diagnostic output.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any of the constructor parameters are null.
    /// </exception>
    public UnitOfWork(
        TContext db,
        IOptions<UnitOfWorkConfiguration> config,
        Func<Models.Environment, IOutboxWriter> writeFactory,
        ILogger<UnitOfWork<TContext>> logger)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(writeFactory);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
        _context = db;
        _writers = config.Value.GetEnvironments().ToDictionary(
            env => env,
            env => writeFactory(env)
        ).ToFrozenDictionary();

        _transaction = db.Database.BeginTransaction(IsolationLevel.ReadCommitted);

        _logger.LogInformation("Transaction started for message broadcasting");
    }

    /// <summary>
    /// Gets all outbox writers created for the configured environments.
    /// </summary>
    public IReadOnlyCollection<IOutboxWriter> Writers => _writers.Values;

    /// <summary>
    /// Saves all pending changes and commits the transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task that completes when the commit operation finishes.</returns>
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Saving changes and committing transaction");
        _ = await _context.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes the transaction scope associated with this unit of work.
    /// </summary>
    /// <returns>A task that completes when disposal is done.</returns>
    public ValueTask DisposeAsync()
    {
        _logger.LogInformation("Disposing transaction scope");
        return _transaction.DisposeAsync();
    }

    private readonly FrozenDictionary<Models.Environment, IOutboxWriter> _writers;
    private readonly TContext _context;
    private readonly IDbContextTransaction _transaction;
    private readonly ILogger _logger;
}

