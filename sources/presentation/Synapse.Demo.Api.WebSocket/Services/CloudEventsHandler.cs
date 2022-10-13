namespace Synapse.Demo.Api.WebSocket.Services;

/// <summary>
/// The service used to handle <see cref="CloudEvent"/>s and forward the <see cref="Integration.IntegrationEvent"/>s on the SignalR <see cref="Hub"/>
/// </summary>
public class CloudEventsHandler
    : BackgroundService
{
    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; init; }

    /// <summary>
    /// Gets the application <see cref="Hub{TClient}"/>
    /// </summary>
    protected IHubContext<DemoApplicationHub, IDemoApplicationClient> HubContext { get; init; }

    /// <summary>
    /// Gets the service used to map objects
    /// </summary>
    protected IMapper Mapper { get; init; }

    /// <summary>
    /// Gets the <see cref="ISubject{T}"/> used to observe consumed <see cref="CloudEvent"/>s
    /// </summary>
    protected ISubject<CloudEvent> Stream { get; init; }

    /// <summary>
    /// Gets the types of the integration assembly inheriting <see cref="Integration.IntegrationEvent"/>
    /// </summary>
    protected IEnumerable<Type> IntegrationEvents { get; init; } = new List<Type>();

    /// <summary>
    /// Gets a <see cref="List{T}"/> containing all registered <see cref="CloudEvent"/> subscriptions
    /// </summary>
    protected List<IDisposable> Subscriptions { get; init; } = new();

    /// <summary>
    /// Gets the subject emitting when the service is disposed
    /// </summary>
    protected Subject<bool> DisposeNotifier { get; init; } = null!;

    /// <summary>
    /// Initiliazes a new <see cref="CloudEventsHandler"/>
    /// </summary>

    /// <param name="logger">The <see cref="ILogger"/> used to log</param>
    /// <param name="hubContext">The application <see cref="Hub{TClient}"/></param>
    /// <param name="mapper">The service used to map objects</param>
    /// <param name="stream">The <see cref="CloudEvent"/>s stream</param>
    public CloudEventsHandler(ILogger<CloudEventsHandler> logger, IHubContext<DemoApplicationHub, IDemoApplicationClient> hubContext, IMapper mapper, ISubject<CloudEvent> stream)
    {
        if (logger == null) throw DomainException.ArgumentNull(nameof(logger));
        if (hubContext == null) throw DomainException.ArgumentNull(nameof(hubContext));
        if (mapper == null) throw DomainException.ArgumentNull(nameof(mapper));
        if (stream == null) throw DomainException.ArgumentNull(nameof(stream));
        this.Logger = logger;
        this.HubContext = hubContext;
        this.Mapper = mapper;
        this.Stream = stream;
        this.DisposeNotifier = new Subject<bool>();
        var integrationEventInterface = typeof(Integration.IIntegrationEvent);
        this.IntegrationEvents = TypeCacheUtil.FindFilteredTypes(
                "integration:events",
                t => !t.IsAbstract
                    && !t.IsInterface
                    && t.IsClass
                    && integrationEventInterface.IsAssignableFrom(t)
                    && t.TryGetCustomAttribute<CloudEventEnvelopeAttribute>(out _),
                typeof(Integration.IIntegrationEvent).Assembly
            );
    }

    /// <summary>
    /// Creates subscriptions for commands sent over the <see cref="ICloudEventBus"/>
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        foreach (Type IntegrationEventType in this.IntegrationEvents)
        {
            if (IntegrationEventType == null) continue;
            var cloudEventEnvelopeAttribute = IntegrationEventType.GetCustomAttribute<CloudEventEnvelopeAttribute>();
            if (cloudEventEnvelopeAttribute == null
                || string.IsNullOrWhiteSpace(cloudEventEnvelopeAttribute.AggregateType)
                || string.IsNullOrWhiteSpace(cloudEventEnvelopeAttribute.ActionName)
            ) continue;
            var expectedCloudEventType = $"{ApplicationConstants.CloudEventsType}/{cloudEventEnvelopeAttribute.AggregateType}/{cloudEventEnvelopeAttribute.ActionName}/v1";
            this.Subscriptions.Add(
                this.Stream
                    .Where(cloudEvent => cloudEvent.Type == expectedCloudEventType)
                    .TakeUntil(this.DisposeNotifier)
                    .Subscribe(async (cloudEvent) =>
                    {
                        try
                        {
                            await this.HubContext.Clients.All.ReceiveIntegrationEventAsync(this.Mapper.Map<CloudEventDto>(cloudEvent));
                        }
                        catch (Exception ex)
                        {
                            this.Logger.LogError($"Failed to forward cloud event command of type '{cloudEvent.Type}'", ex);
                        }
                    })
            );
        }
        await Task.CompletedTask;
    }

    private bool disposed;
    /// <summary>
    /// Disposes the current object
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                this.DisposeNotifier.OnNext(true);
                this.DisposeNotifier.OnCompleted();
                this.Subscriptions.ForEach(subscription => subscription?.Dispose()); // Shouldn't be necessary thanks to `DisposeNotifier`
                this.Subscriptions.Clear();
            }
            disposed = true;
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
