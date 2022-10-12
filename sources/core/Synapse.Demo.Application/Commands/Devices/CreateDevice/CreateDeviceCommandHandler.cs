namespace Synapse.Demo.Application.Commands.Devices;

// TODO: Write tests
/// <summary>
/// Represents a <see cref="ICommandHandler"/> used to handle the <see cref="CreateDeviceCommand"/>
/// </summary>
internal class CreateDeviceCommandHandler
    : CommandHandlerBase, ICommandHandler<CreateDeviceCommand, Device>
{
    /// <summary>
    /// Gets the <see cref="IRepository"/> used to manage <see cref="Domain.Models.Device"/>s
    /// </summary>
    protected IRepository<Domain.Models.Device> Devices { get; init; }

    /// <summary>
    /// Initializes a new <see cref="CreateDeviceCommandHandler"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    /// <param name="devices">The <see cref="IRepository"/> used to manage <see cref="Domain.Models.Device"/>s</param>
    public CreateDeviceCommandHandler(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper
        , IRepository<Domain.Models.Device> devices) 
        : base(loggerFactory, mediator, mapper)
    {
        this.Devices = devices;
    }

    /// <inheritdoc/>
    public async Task<IOperationResult<Device>> HandleAsync(CreateDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var device = new Domain.Models.Device(command.Id, command.Label, command.Type, command.Location, command.State);
        await this.Devices.AddAsync(device, cancellationToken);
        await this.Devices.SaveChangesAsync(cancellationToken);
        device = await this.Devices.FindAsync(command.Id, cancellationToken);
        if (device == null)
        {
            throw new DomainException($"A device with id '{command.Id}' should have been created but cannot be found.");
        }
        var deviceDto = this.Mapper.Map<Device>(device);
        return this.Ok(deviceDto);
    }
}
