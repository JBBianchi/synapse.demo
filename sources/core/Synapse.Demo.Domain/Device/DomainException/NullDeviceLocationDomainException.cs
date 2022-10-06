namespace Synapse.Demo.Domain.DomainException.Device;

/// <summary>
/// The <see cref="Exception"/> thrown when the location of a <see cref="Device"/> is null or empty
/// </summary>
public class NullDeviceLocationDomainException
    : Exception
{
    public NullDeviceLocationDomainException()
        : base("A device type cannot be null or empty.") 
    { }
}
