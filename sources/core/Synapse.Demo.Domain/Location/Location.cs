namespace Synapse.Demo.Domain;

/// <summary>
/// Represents a location and its logical hierarchy
/// </summary>
public record class Location
{
    /// <summary>
    /// The characters used to split the hierarchy
    /// </summary>
    public readonly static string LabelSeparator = "\\\\";

    public Guid Id { get; init; }
    /// <summary>
    /// Gets the label that identifies the <see cref="Location"/>
    /// </summary>
    public string Label { get; init; }
    /// <summary>
    /// Gets the potential parent <see cref="Location"/>
    /// </summary>
    public Location? Parent { get; init; }

    /// <summary>
    /// Constructs a new <see cref="Location"/>
    /// </summary>
    protected Location() {
        this.Label = "";
    }

    /// <summary>
    /// Constructs a new <see cref="Location"/> with the provided label and parent.
    /// </summary>
    /// <param name="label">The label of the location</param>
    /// <param name="parent">The optional parent <see cref="Location"/></param>
    public Location(string label, Location? parent = null)
    {
        if (string.IsNullOrWhiteSpace(label)) throw new NullLocationLabelDomainException();
        if (label.IndexOf(LabelSeparator) > -1) throw new InvalidLocationLabelDomainException(label);
        this.Label = label;
        this.Parent = parent;
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
        if (this.Parent == null) return this.Label.ToString();
        return this.Parent.ToString() + LabelSeparator + this.Label.ToString();
    }
}
