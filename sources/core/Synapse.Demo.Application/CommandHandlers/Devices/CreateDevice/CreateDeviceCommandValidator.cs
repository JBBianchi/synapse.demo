namespace Synapse.Demo.Application.CommandHandlers.Devices.CreateDevice;

// TODO: Write tests
/// <summary>
/// Represents the <see cref="IValidator"/> used to validate the <see cref="CreateDeviceCommand"/>
/// </summary>
internal class CreateDeviceCommandValidator
    : AbstractValidator<CreateDeviceCommand>
{
    /// <summary>
    /// Initializes a new <see cref="CreateDeviceCommandValidator"/>
    /// </summary>
    public CreateDeviceCommandValidator()
    {
        this.RuleFor(command => command.Id)
            .NotEmpty();
        this.RuleFor(command => command.Label)
            .NotEmpty();
        this.RuleFor(command => command.Type)
            .NotEmpty();
        this.RuleFor(command => command.Location)
            .NotEmpty();
    }
}
