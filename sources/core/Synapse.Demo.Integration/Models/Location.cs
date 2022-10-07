namespace Synapse.Demo.Integration.Models;

/// <summary>
/// Represents a location and its logical hierarchy
/// </summary>
[DataTransferObjectFor(typeof(Domain.Models.Location))]
public class Location
{
    /// <summary>
    /// Gets the label that identifies the <see cref="Location"/>
    /// </summary>
    public string Label { get; init; } = null!;
    /// <summary>
    /// Gets the potential parent <see cref="Location"/>
    /// </summary>
    public Location? Parent { get; init; } = null;
}
