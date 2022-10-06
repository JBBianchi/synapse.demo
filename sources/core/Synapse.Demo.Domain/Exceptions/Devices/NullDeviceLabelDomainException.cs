namespace Synapse.Demo.Domain.Exceptions.Devices;

/// <summary>
/// The <see cref="Exception"/> thrown when the label of a <see cref="Device"/> is null or empty
/// </summary>
public class NullDeviceLabelDomainException
    : Exception
{
    public NullDeviceLabelDomainException()
        : base("A device label cannot be null or empty.")
    { }
}
