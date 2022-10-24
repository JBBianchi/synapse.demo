namespace Synapse.Demo.Integration.Commands.Devices;

// TODO: Write tests
/// <summary>
/// Represents the command DTO used to patch the state of a <see cref="Device"/>
/// </summary>
[CloudEventEnvelope("device", "patch-state")]
public class PatchDeviceStateCommand
    : IIntegrationCommand
{
    /// <summary>
    /// Gets the id of the <see cref="Device"/> to create
    /// </summary>
    public string DeviceId { get; init; }

    /// <summary>
    /// Gets the state of the <see cref="Device"/> to create
    /// </summary>
    public object? State { get; init; }

    /// <summary>
    /// Initializes a new <see cref="UpdateDeviceStateCommand"/>
    /// </summary>
    /// <param name="deviceId">The id of the <see cref="Device"/> to change the state of</param>
    /// <param name="state">The partial modified state of the <see cref="Device"/></param>
    public PatchDeviceStateCommand(string deviceId, object? state)
    {
        if (string.IsNullOrWhiteSpace(deviceId)) throw DomainException.ArgumentNullOrWhitespace(nameof(deviceId));
        this.DeviceId = deviceId;
        this.State = state;
    }
}
