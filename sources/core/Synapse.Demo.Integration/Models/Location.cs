namespace Synapse.Demo.Integration.Models;

/// <summary>
/// Represents a location and its logical hierarchy
/// </summary>
public class Location
{
    /// <summary>
    /// The characters used to split the hierarchy
    /// </summary>
    public readonly static string LabelSeparator = ".";

    /// <summary>
    /// Gets the label that identifies the <see cref="Location"/>
    /// </summary>
    public string Label { get; init; } = null!;
    /// <summary>
    /// Gets the potential parent <see cref="Location"/>
    /// </summary>
    public Location? Parent { get; init; } = null;

    /// <summary>
    /// Returns the string representation of the <see cref="Location"/>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (Parent == null) return Label.ToString();
        return Parent.ToString() + LabelSeparator + Label.ToString();
    }
}
// TODO: review ToString and Integration vs Domain
