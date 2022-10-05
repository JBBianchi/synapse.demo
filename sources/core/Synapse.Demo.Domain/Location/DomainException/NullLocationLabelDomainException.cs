namespace Synapse.Demo.Domain.DomainException.Location;

public class NullLocationLabelDomainException
    : Exception
{
    public NullLocationLabelDomainException()
        : base($"A location label cannot be null or empty.")
    { }
}
