namespace Synapse.Demo.WebUI;

/// <summary>
/// The service used to manage knobs
/// </summary>
public interface IKnobManager
{
    /// <summary>
    /// Creates a knob with the provided <see cref="ElementReference"/>
    /// </summary>
    /// <param name="knobRef">The <see cref="ElementReference"/> to create the knob at</param>
    /// <param name="min">The minimum value of the knob</param>
    /// <param name="max">The maximum value of the knob</param>
    /// <param name="value">The current displayed value of the knob</param>
    /// <param name="icon">A material icon to display, if any</param>
    /// <returns></returns>
    Task<IJSObjectReference> CreateKnobAsync(ElementReference knobRef, int min, int max, int value, string? icon = null);
}