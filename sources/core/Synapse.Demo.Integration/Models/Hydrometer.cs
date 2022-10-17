namespace Synapse.Demo.Integration.Models;

/// <summary>
/// Represents a <see cref="Device"/> of type `sensor.hydrometer`
/// </summary>
public class Hydrometer
    : DeviceBase
{
    /// <summary>
    /// Gets the measured humidity
    /// </summary>
    public int? Humidity { get; set; }
    /// <summary>
    /// Gets the measured humidity as string
    /// </summary>
    public string DisplayedHumidity => this.Humidity != null ? $"{this.Humidity}%" : "N/A";
}
