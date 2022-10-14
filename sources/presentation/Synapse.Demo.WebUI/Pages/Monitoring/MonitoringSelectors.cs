using Synapse.Demo.WebUI.Pages.Monitoring.State;

namespace Synapse.Demo.WebUI.Pages.Monitoring.Selectors;

/// <summary>
/// Holds the state slices selectors for the <see cref="MonitoringState"/>
/// </summary>
public static class MonitoringSelectors
{
    /// <summary>
    /// Selects the list of <see cref="Device"/>s
    /// </summary>
    /// <param name="store"></param>
    /// <returns></returns>
    public static IObservable<IEnumerable<Device>> SelectDevices(this IFeature<MonitoringState> feature)
    {
        return feature.Select(featureState => 
                featureState.Devices.Any() ? featureState.Devices.Values : new List<Device>().AsEnumerable()
            )
            .DistinctUntilChanged();
    }
}
