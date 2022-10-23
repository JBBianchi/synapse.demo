namespace Synapse.Demo.Application.Commands.Devices;

// TODO: Write tests
/// <summary>
/// Represents a <see cref="ICommandHandler"/> used to handle the <see cref="UpdateDeviceStateCommand"/>
/// </summary>
internal class UpdateDeviceStateCommandHandler
    : CommandHandlerBase, ICommandHandler<UpdateDeviceStateCommand, Device>
{
    /// <summary>
    /// Gets the <see cref="IRepository"/> used to manage <see cref="Domain.Models.Device"/>s
    /// </summary>
    protected IRepository<Domain.Models.Device> Devices { get; init; }

    /// <summary>
    /// Initializes a new <see cref="UpdateDeviceStateCommandHandler"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    /// <param name="devices">The <see cref="IRepository"/> used to manage <see cref="Domain.Models.Device"/>s</param>
    public UpdateDeviceStateCommandHandler(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper
        , IRepository<Domain.Models.Device> devices) 
        : base(loggerFactory, mediator, mapper)
    {
        this.Devices = devices;
    }

    /// <inheritdoc/>
    public async Task<IOperationResult<Device>> HandleAsync(UpdateDeviceStateCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var device = await this.Devices.FindAsync(command.DeviceId, cancellationToken);
            if (device == null) throw DomainException.NullReference(typeof(Device), command.DeviceId);
            device.SetState(command.State);
            device = await this.Devices.UpdateAsync(device, cancellationToken);
            await this.Devices.SaveChangesAsync(cancellationToken);
            var deviceDto = this.Mapper.Map<Device>(device);
            return this.Ok(deviceDto);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
