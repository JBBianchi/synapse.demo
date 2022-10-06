using System.Net.Mime;

namespace Synapse.Demo.Application.DomainEventHandlers;

/// <summary>
/// Represents the base class for all <see cref="INotificationHandler{TNotification}"/> used to handle <see cref="IDomainEvent"/>s
/// </summary>
internal class DomainEventHandlerBase
{
    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger _logger { get; }

    /// <summary>
    /// Gets the service used to map objects
    /// </summary>
    protected IMapper _mapper { get; }

    /// <summary>
    /// Gets the service used to mediate calls
    /// </summary>
    protected IMediator _mediator { get; }

    /// <summary>
    /// Gets the service used to publish and subscribe to <see cref="CloudEvent"/>s
    /// </summary>
    protected ICloudEventBus _cloudEventBus { get; }

    /// <summary>
    /// Initializes a new <see cref="DomainEventHandlerBase"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mapper">The service used to map objects</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="cloudEventBus">The service used to publish and subscribe to <see cref="CloudEvent"/>s</param>
    protected DomainEventHandlerBase(ILoggerFactory loggerFactory, IMapper mapper, IMediator mediator, ICloudEventBus cloudEventBus)
    {
        this._logger = loggerFactory.CreateLogger(this.GetType());
        this._mapper = mapper;
        this._mediator = mediator;
        this._cloudEventBus = cloudEventBus;
    }

    /// <summary>
    /// Publishes the specified a <see cref="IntegrationEvent"/> on the <see cref="IMediator"/> and the <see cref="ICloudEventBus"/>
    /// </summary>
    /// <param name="e">The <see cref="IntegrationEvent"/> to publish</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    protected virtual async Task PublishIntegrationEventAsync<TEvent>(TEvent e, CancellationToken cancellationToken)
        where TEvent : class, IIntegrationEvent
    {
        await this._mediator.PublishAsync(e);
        if (!e.GetType().TryGetCustomAttribute(out CloudEventEnvelopeAttribute couldEventAttribute))
            return;
        var eventIdentifier = $"/{couldEventAttribute.AggregateType}/{couldEventAttribute.ActionName}/v1";
        CloudEvent cloudEvent = new()
        {
            Id = Guid.NewGuid().ToString(),
            Source = new(Environment.GetEnvironmentVariable("CLOUDEVENTS_SOURCE")!),
            Type = $"com.synapse.demo/eventIdentifier",
            Time = e.CreatedAt,
            Subject = e.AggregateId.ToString(),
            DataSchema = new($"{Environment.GetEnvironmentVariable("SCHEMA_REGISTRY_URI")}/{eventIdentifier}", UriKind.RelativeOrAbsolute),
            DataContentType = MediaTypeNames.Application.Json,
            Data = e
        };
        await this._cloudEventBus.PublishAsync(cloudEvent, cancellationToken);
    }
}


/// <summary>
/// Represents the base class for all <see cref="INotificationHandler{TNotification}"/> used to handle <see cref="IDomainEvent"/>s
/// </summary>
/// <typeparam name="TWriteModel">The type of the write model for which to handle <see cref="IDomainEvent"/>s</typeparam>
/// <typeparam name="TReadModel">The type of the read model to project handled <see cref="IDomainEvent"/>s to</typeparam>
/// <typeparam name="TKey">The type of key used to uniquely identify the read and write models</typeparam>
internal abstract class DomainEventHandlerBase<TWriteModel, TReadModel, TKey>
    : DomainEventHandlerBase
    where TWriteModel : class, IAggregateRoot<TKey>
    where TReadModel : class, IIdentifiable<TKey>
    where TKey : IEquatable<TKey>
{

    /// <summary>
    /// Gets the <see cref="IRepository"/> used to manage the write models for which to handle <see cref="IDomainEvent"/>s
    /// </summary>
    protected readonly IRepository<TWriteModel, TKey> _writeModels;

    /// <summary>
    /// Gets the <see cref="IRepository"/> used to manage the read models to project handled <see cref="IDomainEvent"/>s to
    /// </summary>
    protected readonly IRepository<TReadModel, TKey> _readModels;

    /// <summary>
    /// Initializes a new <see cref="DomainEventHandlerBase"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mapper">The service used to map objects</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="cloudEventBus">The service used to publish and subscribe to <see cref="CloudEvent"/>s</param>
    /// <param name="cloudEventHub">The current <see cref="ICloudEventHub"/>'s <see cref="IHubContext{THub, T}"/></param>
    /// <param name="writeModels">The <see cref="IRepository"/> used to manage the write models for which to handle <see cref="IDomainEvent"/>s</param>
    /// <param name="readModels">The <see cref="IRepository"/> used to manage the read models to project handled <see cref="IDomainEvent"/>s to</param>
    protected DomainEventHandlerBase(ILoggerFactory loggerFactory, IMapper mapper, IMediator mediator, ICloudEventBus cloudEventBus,
        IRepository<TWriteModel, TKey> writeModels, IRepository<TReadModel, TKey> readModels)
        : base(loggerFactory, mapper, mediator, cloudEventBus)
    {
        this._writeModels = writeModels;
        this._readModels = readModels;
    }

    /// <summary>
    /// Gets or reconciles the read model for the <see cref="IAggregateRoot"/> with the specified key
    /// </summary>
    /// <param name="aggregateKey">The id of the <see cref="IAggregateRoot"/> to get the read model for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The read model for the <see cref="IAggregateRoot"/> with the specified key</returns>
    protected virtual async Task<TReadModel> GetOrReconcileReadModelForAsync(TKey aggregateKey, CancellationToken cancellationToken)
    {
        TReadModel readModel = await this._readModels.FindAsync(aggregateKey, cancellationToken);
        if (readModel == null)
        {
            TWriteModel writeModel = await this._writeModels.FindAsync(aggregateKey, cancellationToken);
            if (writeModel == null)
            {
                this._logger.LogError("Failed to find a aggregate of type {aggregateType} with the specified key {key}", typeof(TWriteModel), aggregateKey);
                throw new Exception($"Failed to find a aggregate of type {typeof(TWriteModel)} with the specified key {aggregateKey}");
            }
            readModel = await this.ProjectAsync(writeModel, cancellationToken);
            readModel = await this._readModels.AddAsync(readModel, cancellationToken);
            await this._readModels.SaveChangesAsync(cancellationToken);
        }
        return readModel;
    }

    /// <summary>
    /// Projects the specified <see cref="IAggregateRoot"/>
    /// </summary>
    /// <param name="writeModel">The <see cref="IAggregateRoot"/> to project</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The projected <see cref="IAggregateRoot"/></returns>
    protected virtual async Task<TReadModel> ProjectAsync(TWriteModel writeModel, CancellationToken cancellationToken)
    {
        return await Task.FromResult(this._mapper.Map<TReadModel>(writeModel));
    }

}