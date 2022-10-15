namespace Synapse.Demo.WebUI;

/// <summary>
/// Holds the information used to build a knob
/// </summary>
public class KnobHeroViewModel
{
    /// <summary>
    /// Gets the minimum value of the knob
    /// </summary>
    public int Min { get; set; }

    /// <summary>
    /// Gets the maximum value of the knob
    /// </summary>
    public int Max { get; set; }

    /// <summary>
    /// Gets the current value of the knob
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Gets the knob icon
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Initializes a new <see cref="KnobHeroViewModel"/>
    /// </summary>
    /// <param name="min">The minimum value of the knob</param>
    /// <param name="max">The maximum value of the knob</param>
    /// <param name="value">The current value of the knob</param>
    /// <param name="icon">The knob icon</param>
    public KnobHeroViewModel(int min, int max, int value, string? icon = null)
    {
        this.Min = min;
        this.Max = max;
        this.Value = value;
        this.Icon = icon;
    }
}
