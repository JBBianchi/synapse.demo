namespace Synapse.Demo.Integration.Events.Devices;

// TODO: Write tests
/// <summary>
/// The integration event fired after the creation of a <see cref="Domain.Models.Device"/>
/// </summary>
[CloudEventEnvelope("device", "state-changed")]
public class DeviceStateChangedIntegrationEvent
    : IntegrationEvent
{
    /// <summary>
    /// Gets the id of the created <see cref="Device"/>
    /// </summary>
    public string DeviceId { get; init; }

    /// <summary>
    /// Gets the state of the created <see cref="Device"/>
    /// </summary>
    public object? State { get; init; }

    /// <summary>
    /// Initializes a new <see cref="DeviceStateChangedIntegrationEvent"/>
    /// </summary>
    /// <param name="id">The id of the created <see cref="Device"/></param>
    /// <param name="state">The state of the created <see cref="Device"/></param>
    public DeviceStateChangedIntegrationEvent(string id, object? state)
    {
        this.AggregateId = id;
        this.CreatedAt = DateTime.UtcNow;
        this.DeviceId = id;
        this.State = state;
    }

    /// <summary>
    /// Initializes a new <see cref="DeviceStateChangedIntegrationEvent"/>
    /// </summary>
    protected DeviceStateChangedIntegrationEvent() { }
}
