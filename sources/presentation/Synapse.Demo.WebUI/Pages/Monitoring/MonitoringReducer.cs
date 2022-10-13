using Synapse.Demo.WebUI.Pages.Monitoring.Actions;
using Synapse.Demo.WebUI.Pages.Monitoring.State;

namespace Synapse.Demo.WebUI.Pages.Monitoring.Reducer;

[Reducer]
public class MonitoringReducer
{
    /// <summary>
    /// Replaces the list of <see cref="Device"/>s
    /// </summary>
    /// <param name="state">The state to reduce</param>
    /// <param name="action">The action to reduce</param>
    /// <returns>The reduced state</returns>
    public MonitoringState On(MonitoringState state, ReplaceDevices action)
    {
        return state with {
            Devices = action.Devices.ToDictionary(device => device.Id)
        };
    }

    /// <summary>
    /// Adds <see cref="Device"/> to the list
    /// </summary>
    /// <param name="state">The state to reduce</param>
    /// <param name="action">The action to reduce</param>
    /// <returns>The reduced state</returns>
    public MonitoringState On(MonitoringState state, AddDevice action)
    {
        if (action?.Device == null) return state;
        var devices = new Dictionary<string, Device>(state.Devices);
        devices.Add(action.Device.Id, action.Device);
        return state with
        {
            Devices = devices
        };
    }

    /// <summary>
    /// Updates a <see cref="Device"/> state
    /// </summary>
    /// <param name="state">The state to reduce</param>
    /// <param name="action">The action to reduce</param>
    /// <returns>The reduced state</returns>
    public MonitoringState On(MonitoringState state, UpdateDeviceState action)
    {
        if (action?.DeviceId == null) return state;
        if (!state.Devices.ContainsKey(action.DeviceId)) return state;
        var devices = new Dictionary<string, Device>(state.Devices);
        var device = devices[action.DeviceId];
        device.State = action.State;
        devices.Remove(action.DeviceId);
        devices.Add(action.DeviceId, device);
        return state with
        {
            Devices = devices
        };
    }
}
