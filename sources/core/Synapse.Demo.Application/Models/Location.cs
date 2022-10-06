namespace Synapse.Demo.Application.Models;

/// <summary>
/// Represents a location and its logical hierarchy
/// </summary>
public class Location
{
    /// <summary>
    /// Gets the label that identifies the <see cref="Location"/>
    /// </summary>
    public string Label { get; init; }
    /// <summary>
    /// Gets the potential parent <see cref="Location"/>
    /// </summary>
    public Location? Parent { get; init; }
}
