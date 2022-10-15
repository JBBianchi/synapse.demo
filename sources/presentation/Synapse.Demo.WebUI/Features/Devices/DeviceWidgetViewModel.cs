namespace Synapse.Demo.WebUI;

/// <summary>
/// Holds the data to present a <see cref="Device"/> in a <see cref="DeviceWidget"/> component
/// </summary>
public class DeviceWidgetViewModel
{
    /// <summary>
    /// Gets the id of the device
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// Gets the label of the device
    /// </summary>
    public string Label { get; set; } = null!;

    /// <summary>
    /// Gets the data to display
    /// </summary>
    public string Data { get; set; } = null!;

    /// <summary>
    /// Gets the content of the hero header
    /// </summary>
    public object? Hero { get; set; } = null;

    /// <summary>
    /// Gets if the device is active
    /// </summary>
    public bool IsActive { get; set; } = false;
}
