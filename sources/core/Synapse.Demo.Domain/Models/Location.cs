namespace Synapse.Demo.Domain.Models;

/// <summary>
/// Represents a location and its logical hierarchy
/// </summary>
[DataTransferObjectType(typeof(Integration.Models.Location))]
public record class Location
{
    /// <summary>
    /// The characters used to split the hierarchy
    /// </summary>
    public readonly static string LabelSeparator = "\\\\";

    /// <summary>
    /// Gets the label that identifies the <see cref="Location"/>
    /// </summary>
    public string Label { get; init; } = null!;
    /// <summary>
    /// Gets the potential parent <see cref="Location"/>
    /// </summary>
    public Location? Parent { get; init; } = null;

    /// <summary>
    /// Initializes a new <see cref="Location"/>
    /// </summary>
    protected Location()
    {}

    /// <summary>
    /// Initializes a new <see cref="Location"/> with the provided label and parent.
    /// </summary>
    /// <param name="label">The label of the location</param>
    /// <param name="parent">The optional parent <see cref="Location"/></param>
    public Location(string label, Location? parent = null)
    {
        if (string.IsNullOrWhiteSpace(label)) throw new NullLocationLabelDomainException();
        if (label.IndexOf(LabelSeparator) > -1) throw new InvalidLocationLabelDomainException(label);
        Label = label;
        Parent = parent;
    }

    /// <summary>
    /// Creates a new <see cref="Location"/> based on the provided location string representation
    /// </summary>
    /// <param name="locationStringRepresentation">A location string, may include its hierarchy</param>
    /// <returns>A new <see cref="Location"/></returns>
    public static Location FromString(string locationStringRepresentation)
    {
        var labelIndex = locationStringRepresentation.LastIndexOf(LabelSeparator, StringComparison.Ordinal);
        if (labelIndex == -1)
        {
            return new Location(locationStringRepresentation);
        }
        var parentLabel = locationStringRepresentation.Substring(0, labelIndex);
        var label = locationStringRepresentation.Substring(labelIndex + LabelSeparator.Length);
        return new Location(label, FromString(parentLabel));
    }

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
