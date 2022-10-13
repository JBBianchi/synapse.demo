using Synapse.Demo.WebUI.Pages.Monitoring.Actions;

namespace Synapse.Demo.WebUI.Pages.Monitoring.Effects;

[Effect]
public static class MonitoringEffects
{

    /// <summary>
    /// Handles the state initialization
    /// </summary>
    /// <param name="action">The <see cref="InitializeState"/> action</param>
    /// <param name="context">The <see cref="IEffectContext"/> context</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static async Task OnInitiliazeState(InitializeState action, IEffectContext context)
    {
        // TODO: call the REST API client to get the list of devices
    }
}