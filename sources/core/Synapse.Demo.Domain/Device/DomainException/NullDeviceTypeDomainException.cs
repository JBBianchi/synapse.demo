namespace Synapse.Demo.Domain.DomainException.Device;

public class NullDeviceTypeDomainException
    : Exception
{
    public NullDeviceTypeDomainException()
        : base("A device type cannot be null or empty.") 
    { }
}
