using Synapse.Demo.Integration.Commands;

namespace Synapse.Demo.Application.Services;

/// <summary>
/// The service used to handle incomming <see cref="CloudEvent"/>s and execute incomming <see cref="Command"/>s
/// </summary>
public class CloudEventsHandler
    : BackgroundService
{
    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the service used to map objects
    /// </summary>
    protected IMapper Mapper { get; init; }

    /// <summary>
    /// Gets the <see cref="ISubject{T}"/> used to observe consumed <see cref="CloudEvent"/>s
    /// </summary>
    protected ISubject<CloudEvent> Stream { get; }

    /// <summary>
    /// Gets the types of the integration assembly inheriting <see cref="IIntegrationCommand"/>
    /// </summary>
    protected IEnumerable<Type> IntegrationCommands { get; init; } = new List<Type>();

    /// <summary>
    /// Gets a <see cref="Dictionary{T1,T2}"/> used to retrieve an application <see cref="Command"/> based on an <see cref="IIntegrationCommand"/>'s type
    /// </summary>
    protected Dictionary<Type, Type> IntegrationToApplicationCommandTypes { get; init; } = new Dictionary<Type, Type>();

    /// <summary>
    /// Gets a <see cref="List{T}"/> containing all registered <see cref="CloudEvent"/> subscriptions
    /// </summary>
    protected List<IDisposable> Subscriptions { get; } = new();

    /// <summary>
    /// Gets the subject emitting when the service is disposed
    /// </summary>
    protected Subject<bool> DisposeNotifier { get; } = null!;

    /// <summary>
    /// Initiliazes a new <see cref="CloudEventsHandler"/>
    /// </summary>

    /// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
    /// <param name="logger">The <see cref="ILogger"/> used to log</param>
    /// <param name="mapper">The service used to map objects</param>
    /// <param name="stream">The <see cref="CloudEvent"/>s stream</param>
    public CloudEventsHandler(IServiceProvider serviceProvider, ILogger<CloudEventsHandler> logger, IMapper mapper, ISubject<CloudEvent> stream)
    {
        if (serviceProvider == null) throw DomainException.ArgumentNull(nameof(serviceProvider));
        if (logger == null) throw DomainException.ArgumentNull(nameof(logger));
        if (mapper == null) throw DomainException.ArgumentNull(nameof(mapper));
        if (stream == null) throw DomainException.ArgumentNull(nameof(stream));
        this.ServiceProvider = serviceProvider;
        this.Logger = logger;
        this.Mapper = mapper;
        this.Stream = stream;
        this.DisposeNotifier = new Subject<bool>();
        var integrationCommandInterface = typeof(IIntegrationCommand);
        this.IntegrationCommands = TypeCacheUtil.FindFilteredTypes(
                "integration:commands",
                t => t.IsClass
                    && !t.IsAbstract
                    && !t.IsInterface
                    && integrationCommandInterface.IsAssignableFrom(t)
                    && t.TryGetCustomAttribute<CloudEventEnvelopeAttribute>(out _),
                typeof(CloudEventEnvelopeAttribute).Assembly
            );
        this.IntegrationToApplicationCommandTypes = TypeCacheUtil.FindFilteredTypes(
                "application:commands",
                t => !t.IsAbstract
                    && !t.IsInterface
                    && t.IsClass
                    && t.TryGetCustomAttribute<DataTransferObjectTypeAttribute>(out _),
                this.GetType().Assembly
            )
            .ToDictionary<Type, Type>(t => {
                DataTransferObjectTypeAttribute integrationTypeAttribute = t.GetCustomAttribute<DataTransferObjectTypeAttribute>()!;
                return integrationTypeAttribute.Type;
            });
    }

    /// <summary>
    /// Creates subscriptions for commands sent over the <see cref="ICloudEventBus"/>
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        foreach(Type integrationCommandType in this.IntegrationCommands)
        {
            if (integrationCommandType == null || !this.IntegrationToApplicationCommandTypes.ContainsKey(integrationCommandType)) continue;
            var cloudEventEnvelopeAttribute = integrationCommandType.GetCustomAttribute<CloudEventEnvelopeAttribute>();
            if (cloudEventEnvelopeAttribute == null 
                || string.IsNullOrWhiteSpace(cloudEventEnvelopeAttribute.AggregateType) 
                || string.IsNullOrWhiteSpace(cloudEventEnvelopeAttribute.ActionName)
            ) continue;
            var expectedCloudEventType = $"{ApplicationConstants.CloudEventsType}/{cloudEventEnvelopeAttribute.AggregateType}/{cloudEventEnvelopeAttribute.ActionName}/v1";
            var applicationCommandType = this.IntegrationToApplicationCommandTypes[integrationCommandType];
            this.Subscriptions.Add(
                this.Stream
                    .Where(cloudEvent => cloudEvent.Type == expectedCloudEventType && cloudEvent.Data != null)
                    .TakeUntil(this.DisposeNotifier)
                    .Subscribe(async (cloudEvent) =>
                    {
                        try
                        {
                            using var scope = this.ServiceProvider.CreateScope();
                            var integrationCommand = (cloudEvent.Data as JObject)!.ToObject(integrationCommandType)!;
                            var applicationCommand = this.Mapper.Map(integrationCommand, integrationCommandType, applicationCommandType);
                            var responseType = applicationCommandType.BaseType!.GetGenericArguments()[0];
                            var operationResultType = typeof(IOperationResult<>).MakeGenericType(responseType);
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            await mediator.GetType().GetMethod("ExecuteAsync")!.MakeGenericMethod(operationResultType).InvokeAsync(mediator, applicationCommand, cancellationToken);
                        }
                        catch(Exception ex)
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
