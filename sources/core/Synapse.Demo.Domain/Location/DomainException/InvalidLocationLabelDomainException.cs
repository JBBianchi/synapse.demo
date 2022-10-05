namespace Synapse.Demo.Domain.DomainException.Location;

public class InvalidLocationLabelDomainException
    : Exception
{
    public InvalidLocationLabelDomainException(string label)
        : base($"The location label '{label}' is invalid, it cannot contain '{Domain.Location.LabelSeparator}'.")
    { }
}
