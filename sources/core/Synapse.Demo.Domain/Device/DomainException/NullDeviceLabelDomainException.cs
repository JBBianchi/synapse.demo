namespace Synapse.Demo.Domain.DomainException.Device;

public class NullDeviceLabelDomainException
    : Exception
{
    public NullDeviceLabelDomainException()
        : base("A device label cannot be null or empty.") 
    { }
}
