namespace Synapse.Demo.Integration.Models;

/// <summary>
/// Represents a <see cref="Device"/> of type `sensor.thermometer`
/// </summary>
public class Thermometer
    : DeviceBase
{
    /// <summary>
    /// Gets the measured temperature
    /// </summary>
    public int? Temperature { get; set; }
    /// <summary>
    /// Gets the measured temperature as string
    /// </summary>
    public string DisplayedTemperature => this.Temperature != null ? $"{this.Temperature}°C" : "N/A";
    /// <summary>
    /// Gets the desired temperature
    /// </summary>
    public int? DesiredTemperature { get; set; }
    /// <summary>
    /// Gets the desired temperature as string
    /// </summary>
    public string DisplayedDesiredTemperature => this.DesiredTemperature != null ? $"{this.DesiredTemperature}°C" : "N/A";
}
