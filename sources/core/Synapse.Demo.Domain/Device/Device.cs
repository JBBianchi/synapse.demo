namespace Synapse.Demo.Domain;

/// <summary>
/// Represents an IoT device
/// </summary>
public class Device
        : AggregateRoot
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
    /// Constructs a new <see cref="Device"/>
    /// </summary>
    protected Device()
        : base("")
    {
        this.Label = "";
        this.Type = "";
        this.Location = "";
        this.State = null;
    }

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
        if (string.IsNullOrWhiteSpace(id)) throw new NullDeviceIdDomainException();
        if (string.IsNullOrWhiteSpace(label)) throw new NullDeviceLabelDomainException();
        if (string.IsNullOrWhiteSpace(type)) throw new NullDeviceTypeDomainException();
        if (string.IsNullOrWhiteSpace(location)) throw new NullDeviceLocationDomainException();
        /*
        this.Id = id;
        this.CreatedAt = DateTime.UtcNow;
        this.LastModified = DateTime.UtcNow;
        this.Label = label;
        this.Type = type;
        this.Location = location;
        this.State = state;        
        this.RegisterEvent(new DeviceCreatedDomainEvent(
            id: this.Id, 
            label: this.Label, 
            type: this.Type, 
            location: this.Location, 
            state: this.State
        ));
         */
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
        this.On(this.RegisterEvent(new DeviceStateChangedDomainEvent(this.Id, state)));
    }

    /// <summary>
    /// Handles a <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    /// <param name="e">The <see cref="Domain.DomainEvent"/> to handle</param>
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
    /// <param name="e">The <see cref="Domain.DomainEvent"/> to handle</param>
    protected void On(DeviceStateChangedDomainEvent e)
    {
        this.State = e.State;
        this.LastModified = e.CreatedAt;
    }

}
