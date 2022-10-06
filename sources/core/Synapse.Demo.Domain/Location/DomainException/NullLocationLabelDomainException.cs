namespace Synapse.Demo.Domain.DomainException.Location;

/// <summary>
/// The <see cref="Exception"/> thrown when the label of a <see cref="Location"/> is null or empty
/// </summary>
public class NullLocationLabelDomainException
    : Exception
{
    public NullLocationLabelDomainException()
        : base($"A location label cannot be null or empty.")
    { }
}
