namespace Synapse.Demo.Domain.DomainException.Device;

public class NullDeviceLocationDomainException
    : Exception
{
    public NullDeviceLocationDomainException()
        : base("A device type cannot be null or empty.") 
    { }
}
