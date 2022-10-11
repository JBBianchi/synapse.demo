namespace Synapse.Demo.Integration.Commands.Devices;

// TODO: Write tests
/// <summary>
/// Represents the command DTO used to update the state of a <see cref="Device"/>
/// </summary>
public class UpdateDeviceStateCommand
{
    /// <summary>
    /// Gets the id of the <see cref="Device"/> to create
    /// </summary>
    public string DeviceId { get; init; }

    /// <summary>
    /// Gets the state of the <see cref="Device"/> to create
    /// </summary>
    public object? State { get; init; }

    public UpdateDeviceStateCommand(string deviceId, object? state)
    {
        if (string.IsNullOrWhiteSpace(deviceId)) throw DomainException.ArgumentNullOrWhitespace(nameof(deviceId));
        this.DeviceId = deviceId;
        this.State = state;
    }
}
