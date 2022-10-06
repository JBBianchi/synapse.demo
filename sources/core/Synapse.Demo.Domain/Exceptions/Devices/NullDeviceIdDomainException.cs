namespace Synapse.Demo.Domain.Exceptions.Devices;

/// <summary>
/// The <see cref="Exception"/> thrown when the id of a <see cref="Device"/> is null or empty
/// </summary>
public class NullDeviceIdDomainException
    : Exception
{
    public NullDeviceIdDomainException()
        : base("A device id cannot be null or empty.")
    { }
}
