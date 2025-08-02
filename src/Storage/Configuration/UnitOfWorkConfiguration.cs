using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

using Microsoft.Extensions.Options;

namespace Storage.Configuration;

/// <summary>
/// Configuration model that defines the list of environment names
/// used to determine which outbox tables to write to.
/// </summary>
public class UnitOfWorkConfiguration : IValidateOptions<UnitOfWorkConfiguration>
{
    /// <summary>
    /// Gets or sets the list of environment identifiers (e.g., "DEV", "STAGE", "PROD").
    /// Each environment corresponds to an outbox table named like "outbox_{env}".
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one outbox environment must be defined.")]

    public required Collection<string> OutboxEnvironments { get; init; }

    public IEnumerable<Models.Environment> GetEnvironments() => OutboxEnvironments.Select(x => new Models.Environment(x));

    /// <summary>
    /// Validates the configuration.
    /// </summary>
    public ValidateOptionsResult Validate(string? name, UnitOfWorkConfiguration options)
    {
        Debug.Assert(options != null);

        if (options.OutboxEnvironments == null || options.OutboxEnvironments.Count == 0)
        {
            return ValidateOptionsResult.Fail("OutboxEnvironments section is missing or empty.");
        }

        if (options.OutboxEnvironments.Any(string.IsNullOrWhiteSpace))
        {
            return ValidateOptionsResult.Fail("OutboxEnvironments cannot contain empty or whitespace values.");
        }

        return ValidateOptionsResult.Success;
    }
}
