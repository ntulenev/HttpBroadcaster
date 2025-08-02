namespace Models;

/// <summary>
/// Represents the target environment (e.g., "PROD", "STAGING").
/// </summary>
public sealed class Environment
{
    /// <summary>
    /// Gets the normalized name of the environment (e.g., "PROD").
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Environment"/> class.
    /// </summary>
    /// <param name="value">The environment name.</param>
    /// <exception cref="ArgumentException">Thrown if the value is null or whitespace.</exception>
    public Environment(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim().ToUpperInvariant();
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Environment other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode(StringComparison.InvariantCulture);
}
