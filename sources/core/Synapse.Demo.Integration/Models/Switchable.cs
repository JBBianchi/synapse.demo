namespace Synapse.Demo.Integration.Models;

/// <summary>
/// Represents a <see cref="Device"/> that can be turned on or off
/// </summary>
public class Switchable
    : DeviceBase
{
    /// <summary>
    /// Gets if the device is turned on
    /// </summary>
    public bool IsTurnedOn { get; set; }
}
