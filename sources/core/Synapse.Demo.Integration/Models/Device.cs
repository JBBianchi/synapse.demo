namespace Synapse.Demo.Integration.Models;

/// <summary>
/// Represents an IoT device
/// </summary>
[DataTransferObjectFor(typeof(Domain.Models.Device))]
public class Device
    : Entity
{

    /// <summary>
    /// Gets/Sets the label of the <see cref="Device"/>
    /// </summary>
    public string Label { get; set; } = null!;

    /// <summary>
    /// Gets/Sets the type of <see cref="Device"/>
    /// </summary>
    public string Type { get; set; } = null!;

    /// <summary>
    /// Gets/Sets the location of the <see cref="Device"/>
    /// </summary>
    public Location Location { get; set; } = null!;

    /// <summary>
    /// Gets/Sets the state of the <see cref="Device"/>
    /// </summary>
    public object? State { get; set; } = null!;

    /// <summary>
    /// Initializes a new <see cref="Device"/>
    /// </summary>
    protected Device()
        : base(null!)
    {}
}
