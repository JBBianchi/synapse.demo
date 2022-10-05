namespace Synapse.Demo.Domain.DomainEvent.Device;

public class DeviceStateChangedDomainEvent
    : DomainEvent<Domain.Device>
{
    /// <summary>
    /// Gets the state of the <see cref="Device"/>
    /// </summary>
    public object? State { get; protected set; }

    /// <summary>
    /// Constructs a new <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    protected DeviceStateChangedDomainEvent()
        : base("")
    {
        this.State = null;
    }

    /// <summary>
    /// Constructs a new <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    /// <param name="state">The new state of the <see cref="Domain.Device"/></param>
    public DeviceStateChangedDomainEvent(string id, object? state)
        : base(id)
    {
        this.State = state;
    }
}
