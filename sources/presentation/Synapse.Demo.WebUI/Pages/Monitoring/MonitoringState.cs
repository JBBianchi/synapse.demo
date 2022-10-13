namespace Synapse.Demo.WebUI.Pages.Monitoring.State;

/// <summary>
/// The <see cref="State{TState}"/> of the monitoring page
/// </summary>
[Neuroglia.Data.Flux.Feature]
public record MonitoringState
{
    /// <summary>
    /// The <see cref="Device"/>s displayed
    /// </summary>
    public Dictionary<string, Device> Devices { get; set; } = new Dictionary<string, Device>();
}
