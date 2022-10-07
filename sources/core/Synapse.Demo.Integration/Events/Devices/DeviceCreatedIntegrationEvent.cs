namespace Synapse.Demo.Integration.Events.Devices;

// TODO: Write tests
/// <summary>
/// The integration event fired after the creation of a <see cref="Domain.Models.Device"/>
/// </summary>
[CloudEventEnvelope("device", "created")]
public class DeviceCreatedIntegrationEvent
    : IntegrationEvent
{
    /// <summary>
    /// Gets the id of the created <see cref="Device"/>
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets the label of the created <see cref="Device"/>
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// Gets the type of <see created cref="Device"/>
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// Gets the location of the created <see cref="Device"/> 
    /// </summary>
    public Location Location { get; init; }

    /// <summary>
    /// Gets the state of the created <see cref="Device"/>
    /// </summary>
    public object? State { get; init; }

    /// <summary>
    /// Constructs a new <see cref="DeviceCreatedIntegrationEvent"/>
    /// </summary>
    /// <param name="id">The id of the created <see cref="Device"/></param>
    /// <param name="label">The label of the created <see cref="Device"/></param>
    /// <param name="type">The type of the created <see cref="Device"/></param>
    /// <param name="location">The location of the created <see cref="Device"/></param>
    /// <param name="state">The state of the created <see cref="Device"/></param>
    public DeviceCreatedIntegrationEvent(string id, string label, string type, Location location, object? state)
    {
        this.AggregateId = id;
        this.CreatedAt = DateTime.UtcNow;
        this.Id = id;
        this.Label = label;
        this.Type = type;
        this.Location = location;
        this.State = state;
    }

    /// <summary>
    /// Constructs a new <see cref="DeviceCreatedIntegrationEvent"/>
    /// </summary>
    /// <param name="device">The created <see cref="Device"/></param>
    public DeviceCreatedIntegrationEvent(Device device)
        : this(device.Id, device.Label, device.Type, device.Location, device.State)
    { }

    /// <summary>
    /// Constructs a new <see cref="DeviceCreatedIntegrationEvent"/>
    /// </summary>
    protected DeviceCreatedIntegrationEvent() { }
}
