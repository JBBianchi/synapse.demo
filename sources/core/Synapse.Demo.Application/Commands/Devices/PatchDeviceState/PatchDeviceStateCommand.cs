namespace Synapse.Demo.Application.Commands.Devices;

// TODO: Write tests
/// <summary>
/// Represents the <see cref="ICommand"/> used to patch the state of a <see cref="Device"/>
/// </summary>
[DataTransferObjectType(typeof(Integration.Commands.Devices.PatchDeviceStateCommand))]
public class PatchDeviceStateCommand
    : Command<Device>
{
    /// <summary>
    /// Gets the id of the <see cref="Device"/> to create
    /// </summary>
    public string DeviceId { get; init; }

    /// <summary>
    /// Gets the state of the <see cref="Device"/> to create
    /// </summary>
    public object? State { get; init; }

    public PatchDeviceStateCommand(string deviceId, object? state)
    {
        if (string.IsNullOrWhiteSpace(deviceId)) throw DomainException.ArgumentNullOrWhitespace(nameof(deviceId));
        this.DeviceId = deviceId;
        this.State = state;
    }
}
