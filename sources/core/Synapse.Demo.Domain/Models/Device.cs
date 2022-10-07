namespace Synapse.Demo.Domain.Models;

/// <summary>
/// Represents an IoT device
/// </summary>
public class Device
    : AggregateRoot
{

    /// <summary>
    /// Gets the label of the <see cref="Device"/>
    /// </summary>
    public string Label { get; protected set; } = null!;

    /// <summary>
    /// Gets the type of <see cref="Device"/>
    /// </summary>
    public string Type { get; protected set; } = null!;

    /// <summary>
    /// Gets the location of the <see cref="Device"/>
    /// </summary>
    public string Location { get; protected set; } = null!;

    /// <summary>
    /// Gets the state of the <see cref="Device"/>
    /// </summary>
    public object? State { get; protected set; } = null;

    /// <summary>
    /// Initializes a new <see cref="Device"/>
    /// </summary>
    protected Device()
        : base(null!)
    {}

    /// <summary>
    /// Construct a new <see cref="Device"/>
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Device"/></param>
    /// <param name="label">The label of the <see cref="Device"/></param>
    /// <param name="type">The type of the <see cref="Device"/></param>
    /// <param name="location">The location of the <see cref="Device"/></param>
    /// <param name="state">The state of the <see cref="Device"/></param>
    /// <exception cref="NullDeviceIdDomainException"></exception>
    /// <exception cref="NullDeviceLabelDomainException"></exception>
    /// <exception cref="NullDeviceTypeDomainException"></exception>
    /// <exception cref="NullDeviceLocationDomainException"></exception>
    public Device(string id, string label, string type, string location, object? state)
        : base(id)
    {
        // TODO: replace with generic DomainException ?
        if (string.IsNullOrWhiteSpace(id)) throw new NullDeviceIdDomainException();
        if (string.IsNullOrWhiteSpace(label)) throw new NullDeviceLabelDomainException();
        if (string.IsNullOrWhiteSpace(type)) throw new NullDeviceTypeDomainException();
        if (string.IsNullOrWhiteSpace(location)) throw new NullDeviceLocationDomainException();
        this.On(this.RegisterEvent(new DeviceCreatedDomainEvent(
            id: id,
            label: label,
            type: type,
            location: location,
            state: state
        )));
    }

    /// <summary>
    /// Sets the <see cref="Device"/> state
    /// </summary>
    /// <param name="state"></param>
    public void SetState(object? state)
    {
        this.On(this.RegisterEvent(new DeviceStateChangedDomainEvent(Id, state)));
    }

    /// <summary>
    /// Handles a <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    /// <param name="e">The <see cref="DomainEvent"/> to handle</param>
    protected void On(DeviceCreatedDomainEvent e)
    {
        this.Id = e.AggregateId;
        this.CreatedAt = e.CreatedAt;
        this.LastModified = e.CreatedAt;
        this.Label = e.Label;
        this.Type = e.Type;
        this.Location = e.Location;
        this.State = e.State;
    }

    /// <summary>
    /// Handles a <see cref="DeviceStateChangedDomainEvent"/>
    /// </summary>
    /// <param name="e">The <see cref="DomainEvent"/> to handle</param>
    protected void On(DeviceStateChangedDomainEvent e)
    {
        this.State = e.State;
        this.LastModified = e.CreatedAt;
    }

}
