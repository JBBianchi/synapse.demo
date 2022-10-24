namespace Synapse.Demo.Application.Commands.Devices;

// TODO: Write tests
/// <summary>
/// Represents the <see cref="IValidator"/> used to validate the <see cref="CreateDeviceCommand"/>
/// </summary>
internal class PatchDeviceStateCommandValidator
    : AbstractValidator<PatchDeviceStateCommand>
{
    /// <summary>
    /// Initializes a new <see cref="PatchDeviceStateCommandValidator"/>
    /// </summary>
    public PatchDeviceStateCommandValidator()
    {
        this.RuleFor(command => command.DeviceId)
            .NotEmpty();
    }
}
