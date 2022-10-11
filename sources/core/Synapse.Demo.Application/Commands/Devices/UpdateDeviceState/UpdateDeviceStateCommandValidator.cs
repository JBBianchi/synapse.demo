namespace Synapse.Demo.Application.Commands.Devices;

// TODO: Write tests
/// <summary>
/// Represents the <see cref="IValidator"/> used to validate the <see cref="CreateDeviceCommand"/>
/// </summary>
internal class UpdateDeviceStateCommandValidator
    : AbstractValidator<UpdateDeviceStateCommand>
{
    /// <summary>
    /// Initializes a new <see cref="UpdateDeviceStateCommandValidator"/>
    /// </summary>
    public UpdateDeviceStateCommandValidator()
    {
        this.RuleFor(command => command.DeviceId)
            .NotEmpty();
    }
}
