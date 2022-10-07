namespace Synapse.Demo.Application.DomainEventHandlers.Devices;

// TODO: Write tests
/// <summary>
/// Handles <see cref="IDomainEvent"/>s related to <see cref="Domain.Models.Device"/>s
/// </summary>
internal class DevicesDomainEventsHandler
    : DomainEventHandlerBase<Domain.Models.Device, Device, string>
    , INotificationHandler<DeviceCreatedDomainEvent>
{
    /// <inheritdoc/>
    public DevicesDomainEventsHandler(ILoggerFactory loggerFactory, IMapper mapper, IMediator mediator, ICloudEventBus cloudEventBus, IRepository<Domain.Models.Device, string> writeModels, IRepository<Device, string> readModels) 
        : base(loggerFactory, mapper, mediator, cloudEventBus, writeModels, readModels)
    {}

    /// <summary>
    /// Handles a <see cref="DeviceCreatedDomainEvent"/>
    /// </summary>
    /// <param name="e">The <see cref="DeviceCreatedDomainEvent"/> to handle</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task HandleAsync(DeviceCreatedDomainEvent e, CancellationToken cancellationToken = default)
    {
        await this.GetOrReconcileReadModelForAsync(e.AggregateId, cancellationToken);
        var integrationEvent = this.Mapper.Map<DeviceCreatedIntegrationEvent>(e);
        await this.PublishIntegrationEventAsync(integrationEvent, cancellationToken);
    }
}
