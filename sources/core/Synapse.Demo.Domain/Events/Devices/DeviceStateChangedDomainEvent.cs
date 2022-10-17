namespace Synapse.Demo.Domain.Events.Devices;

/// <summary>
/// The <see cref="DomainEvent"/> fired after the state of a <see cref="Device"/> as changed
/// </summary>
[DataTransferObjectType(typeof(DeviceStateChangedIntegrationEvent))]
public class DeviceStateChangedDomainEvent
    : DomainEvent<Device>
{
    /// <summary>
    /// Gets the id of the <see cref="Device"/> to create
    /// </summary>
    public string DeviceId { get; init; }

    /// <summary>
    /// Gets the state of the <see cref="Device"/>
    /// </summary>
    public object? State { get; protected set; } = null!;

    /// <summary>
    /// Initializes a new <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    protected DeviceStateChangedDomainEvent()
        : base(null!)
    {}

    /// <summary>
    /// Initializes a new <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    /// <param name="state">The new state of the <see cref="Device"/></param>
    public DeviceStateChangedDomainEvent(string id, object? state)
        : base(id)
    {
        this.DeviceId = id;
        this.State = state;
    }
}
