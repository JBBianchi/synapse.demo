namespace Synapse.Demo.Domain.DomainException.Location;

/// <summary>
/// The <see cref="Exception"/> thrown when the label of a <see cref="Location"/> is not in a valid format
/// </summary>
public class InvalidLocationLabelDomainException
    : Exception
{
    public InvalidLocationLabelDomainException(string label)
        : base($"The location label '{label}' is invalid, it cannot contain '{Domain.Location.LabelSeparator}'.")
    { }
}
