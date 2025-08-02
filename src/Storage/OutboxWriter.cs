using Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Storage;

/// <summary>
/// Writes inbound messages to the corresponding outbox table for a specific environment.
/// </summary>
public sealed class OutboxWriter : IOutboxWriter
{
    /// <summary>
    /// Gets the environment associated with this outbox writer.
    /// </summary>
    public Models.Environment Environment { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OutboxWriter"/> class.
    /// </summary>
    /// <param name="environment">The logical environment (e.g., prod, staging).</param>
    /// <param name="db">The database context used for executing SQL commands.</param>
    /// <param name="logger">The logger instance used for structured logging.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="environment"/>, <paramref name="db"/>, or <paramref name="logger"/> is null.
    /// </exception>
    public OutboxWriter(Models.Environment environment, MultiOutboxDbContext db, ILogger<OutboxWriter> logger)
    {
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(logger);

        Environment = environment;
        _db = db;
        _tableName = $"outbox_{environment.Value}";
        _logger = logger;
    }

    /// <summary>
    /// Writes the specified inbound message to the environment's outbox table.
    /// </summary>
    /// <param name="message">The message to write.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the token.</exception>
    public async Task WriteAsync(Models.InboundMessage message, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);
        ct.ThrowIfCancellationRequested();

        var innerMessage = new InboundMessage
        {
            CreatedAt = message.CreatedAt,
            Id = message.Id.Value,
            Payload = message.Payload.Payload
        };

        _logger.LogInformation("Writing event {EventId} to environment '{Environment}'", message.Id, Environment.Value);

        var sql = @$"
            INSERT INTO {_tableName} (Id, Payload, CreatedAt)
            VALUES (@Id, @Payload, @CreatedAt);";

        var sqlParams = new[]
        {
            new Npgsql.NpgsqlParameter("Id", innerMessage.Id),
            new Npgsql.NpgsqlParameter("Payload", innerMessage.Payload),
            new Npgsql.NpgsqlParameter("CreatedAt", innerMessage.CreatedAt)
        };

        _ = await _db.Database.ExecuteSqlRawAsync(sql, sqlParams, ct);
    }

    private readonly MultiOutboxDbContext _db;
    private readonly string _tableName;
    private readonly ILogger _logger;
}
