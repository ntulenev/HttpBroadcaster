using System.Collections.ObjectModel;

namespace Storage.Configuration;

/// <summary>
/// Configuration model that defines the list of environment names
/// used to determine which outbox tables to write to.
/// </summary>
public class UnitOfWorkConfiguration
{
    /// <summary>
    /// Gets or sets the list of environment identifiers (e.g., "DEV", "STAGE", "PROD").
    /// Each environment corresponds to an outbox table named like "outbox_{env}".
    /// </summary>
    public required Collection<string> OutboxEnvironments { get; init; }

    public IEnumerable<Models.Environment> GetEnvironments() => OutboxEnvironments.Select(x => new Models.Environment(x));
}
