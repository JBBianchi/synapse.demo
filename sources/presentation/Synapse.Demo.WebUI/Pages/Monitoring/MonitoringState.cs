namespace Synapse.Demo.WebUI.Pages.Monitoring.State;

/// <summary>
/// The <see cref="State{TState}"/> of the monitoring page
/// </summary>
[Feature]
public record MonitoringState
{
    /// <summary>
    /// Gets the <see cref="Device"/>s displayed
    /// </summary>
    public Dictionary<string, Device> Devices { get; set; } = new Dictionary<string, Device>();

    /// <summary>
    /// Gets the visibility state of the controls panel
    /// </summary>
    public bool AreControlsHidden { get; set; } = true;
}
