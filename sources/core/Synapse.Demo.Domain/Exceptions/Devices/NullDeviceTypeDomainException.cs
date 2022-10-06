namespace Synapse.Demo.Domain.Exceptions.Devices;

/// <summary>
/// The <see cref="Exception"/> thrown when the type of a <see cref="Device"/> is null or empty
/// </summary>
public class NullDeviceTypeDomainException
    : Exception
{
    public NullDeviceTypeDomainException()
        : base("A device type cannot be null or empty.")
    { }
}
