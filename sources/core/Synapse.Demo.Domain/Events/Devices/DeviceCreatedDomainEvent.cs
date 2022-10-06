namespace Synapse.Demo.Domain.Events.Devices;

/// <summary>
/// The <see cref="DomainEvent"/> fired after the creation of a <see cref="Device"/>
/// </summary>
public class DeviceCreatedDomainEvent
    : DomainEvent<Device>
{

    /// <summary>
    /// Gets the label of the <see cref="Device"/>
    /// </summary>
    public string Label { get; protected set; }

    /// <summary>
    /// Gets the type of <see cref="Device"/>
    /// </summary>
    public string Type { get; protected set; }

    /// <summary>
    /// Gets the location of the <see cref="Device"/>
    /// </summary>
    public string Location { get; protected set; }

    /// <summary>
    /// Gets the state of the <see cref="Device"/>
    /// </summary>
    public object? State { get; protected set; }

    /// <summary>
    /// Constructs a new <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    protected DeviceCreatedDomainEvent()
        : base("")
    {
        this.Label = "";
        this.Type = "";
        this.Location = "";
        this.State = null;
    }

    /// <summary>
    /// Constructs a new <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    /// <param name="label">The label of the created <see cref="Device"/></param>
    /// <param name="type">The type of the created <see cref="Device"/></param>
    /// <param name="location">The location of the created <see cref="Device"/></param>
    /// <param name="state">The state of the create <see cref="Device"/></param>
    public DeviceCreatedDomainEvent(string id, string label, string type, string location, object? state)
        : base(id)
    {
        this.Label = label;
        this.Type = type;
        this.Location = location;
        this.State = state;
    }

    /// <summary>
    /// Constructs a new <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    /// <param name="label">The label of the created <see cref="Device"/></param>
    /// <param name="type">The type of the created <see cref="Device"/></param>
    /// <param name="location">The location of the created <see cref="Device"/></param>
    /// <param name="state">The state of the create <see cref="Device"/></param>
    public DeviceCreatedDomainEvent(Device device)
        : this(device.Id, device.Label, device.Type, device.Location, device.State)
    {
    }
}
