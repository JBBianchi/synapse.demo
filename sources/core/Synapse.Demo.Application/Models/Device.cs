namespace Synapse.Demo.Application.Models;

/// <summary>
/// Represents an IoT device
/// </summary>
public class Device
    : Entity
{

    /// <summary>
    /// Gets/Sets the label of the <see cref="Device"/>
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Gets/Sets the type of <see cref="Device"/>
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets/Sets the location of the <see cref="Device"/>
    /// </summary>
    public Location Location { get; set; }

    /// <summary>
    /// Gets/Sets the state of the <see cref="Device"/>
    /// </summary>
    public object? State { get; set; }

    /// <summary>
    /// Constructs a new <see cref="Device"/>
    /// </summary>
    protected Device()
        : base("")
    {
        Label = "";
        Type = "";
        Location = null!;
        State = null;
    }
}
