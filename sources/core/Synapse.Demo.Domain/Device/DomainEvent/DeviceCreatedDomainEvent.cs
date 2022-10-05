namespace Synapse.Demo.Domain.DomainEvent.Device;

public class DeviceCreatedDomainEvent
    : DomainEvent<Domain.Device>
{

    /// <summary>
    /// Gets the label of the <see cref="Domain.Device"/>
    /// </summary>
    public string Label { get; protected set; }

    /// <summary>
    /// Gets the type of <see cref="Domain.Device"/>
    /// </summary>
    public string Type { get; protected set; }

    /// <summary>
    /// Gets the location of the <see cref="Domain.Device"/>
    /// </summary>
    public string Location { get; protected set; }

    /// <summary>
    /// Gets the state of the <see cref="Domain.Device"/>
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
    /// <param name="label">The label of the created <see cref="Domain.Device"/></param>
    /// <param name="type">The type of the created <see cref="Domain.Device"/></param>
    /// <param name="location">The location of the created <see cref="Domain.Device"/></param>
    /// <param name="state">The state of the create <see cref="Domain.Device"/></param>
    public DeviceCreatedDomainEvent(string id, string label, string type, string location, object? state)
        : base(id)
    {
        this.Label = label;
        this.Type = type;
        this.Location = location;
        this.State = state;
    }
}
