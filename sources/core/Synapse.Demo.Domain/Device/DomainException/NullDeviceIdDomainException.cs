namespace Synapse.Demo.Domain.DomainException.Device;

public class NullDeviceIdDomainException
    : Exception
{
    public NullDeviceIdDomainException()
        : base("A device id cannot be null or empty.") 
    { }
}
