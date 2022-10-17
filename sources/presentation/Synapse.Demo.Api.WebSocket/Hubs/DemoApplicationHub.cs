
namespace Synapse.Demo.Api.WebSocket.Hubs;

/// <summary>
/// Represents the WebSocket facade of the application
/// </summary>
public class DemoApplicationHub
    : Hub<IDemoApplicationClient>
{

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; init; }

    /// <summary>
    /// Gets the service used to map objects
    /// </summary>
    protected IMapper Mapper { get; init; }

    /// <summary>
    /// Gets the service used to mediate calls
    /// </summary>
    protected IMediator Mediator { get; init; }

    /// <summary>
    /// Gets the types of the integration assembly inheriting <see cref="IIntegrationCommand"/>
    /// </summary>
    protected IEnumerable<Type> IntegrationCommands { get; init; } = new List<Type>();

    /// <summary>
    /// Gets a nested <see cref="Dictionary{T1,T2}"/> used to retrieve an<see cref="IIntegrationCommand"/>'s type based on <see cref="CloudEventDto"/> type
    /// </summary>
    protected Dictionary<string, Dictionary<string, Type>> CloudToIntegrationTypes { get; init; } = new Dictionary<string, Dictionary<string, Type>>();

    /// <summary>
    /// Gets a <see cref="Dictionary{T1,T2}"/> used to retrieve an application <see cref="Command"/> based on an <see cref="IIntegrationCommand"/>'s type
    /// </summary>
    protected Dictionary<Type, Type> IntegrationToApplicationCommandTypes { get; init; } = new Dictionary<Type, Type>();

    /// <summary>
    /// Initializses a new <see cref="DemoApplicationHub"/>
    /// </summary>
    /// <param name="logger">The service used to perform logging</param>
    /// <param name="mapper">The service used to map objects</param>
    /// <param name="mediator">The service used to mediate calls</param>
    public DemoApplicationHub(ILogger<DemoApplicationHub> logger, IMapper mapper, IMediator mediator)
    {
        if (logger == null) throw DomainException.ArgumentNull(nameof(logger));
        if (mapper == null) throw DomainException.ArgumentNull(nameof(mapper));
        if (mediator == null) throw DomainException.ArgumentNull(nameof(mediator));
        this.Logger = logger;
        this.Mapper = mapper;
        this.Mediator = mediator;
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
        foreach(var integrationCommandType in this.IntegrationCommands)
        {
            var cloudEventEnvelopeAttribute = integrationCommandType.GetCustomAttribute<CloudEventEnvelopeAttribute>();
            if (cloudEventEnvelopeAttribute == null
                || string.IsNullOrWhiteSpace(cloudEventEnvelopeAttribute.AggregateType)
                || string.IsNullOrWhiteSpace(cloudEventEnvelopeAttribute.ActionName)
            ) continue;
            if (!this.CloudToIntegrationTypes.ContainsKey(cloudEventEnvelopeAttribute.AggregateType))
            {
                this.CloudToIntegrationTypes.Add(cloudEventEnvelopeAttribute.AggregateType, new Dictionary<string, Type>());
            }
            var aggregateMap = this.CloudToIntegrationTypes[cloudEventEnvelopeAttribute.AggregateType];
            if (!aggregateMap.ContainsKey(cloudEventEnvelopeAttribute.ActionName))
            {
                aggregateMap.Add(cloudEventEnvelopeAttribute.ActionName, integrationCommandType);
            }
        }
        this.IntegrationToApplicationCommandTypes = TypeCacheUtil.FindFilteredTypes(
                "application:commands",
                t => !t.IsAbstract
                    && !t.IsInterface
                    && t.IsClass
                    && t.TryGetCustomAttribute<DataTransferObjectTypeAttribute>(out _),
                typeof(IDemoApplicationBuilder).Assembly
            )
            .ToDictionary<Type, Type>(t => {
                DataTransferObjectTypeAttribute integrationTypeAttribute = t.GetCustomAttribute<DataTransferObjectTypeAttribute>()!;
                return integrationTypeAttribute.Type;
            });
    }

    /// <summary>
    /// The method used to handle cloud events as integration commands
    /// </summary>
    /// <param name="cloudEvent">The <see cref="CloudEventDto"/> potentially holding a <see cref="IIntegrationCommand"/></param>
    /// <returns></returns>
    public async Task HandleCloudEvent(CloudEventDto cloudEvent)
    {
        if (cloudEvent?.Type == null) return;
        var typeParts = cloudEvent.Type.Split('/');
        var aggregateType = typeParts[1];
        var actionName = typeParts[2];
        if (!this.CloudToIntegrationTypes.ContainsKey(aggregateType) || !this.CloudToIntegrationTypes[aggregateType].ContainsKey(actionName)) return;
        var integrationCommandType = this.CloudToIntegrationTypes[aggregateType][actionName];
        if (integrationCommandType == null || !this.IntegrationToApplicationCommandTypes.ContainsKey(integrationCommandType)) return;
        var integrationCommand = (cloudEvent.Data as JObject)!.ToObject(integrationCommandType)!;
        var applicationCommandType = this.IntegrationToApplicationCommandTypes[integrationCommandType];
        var applicationCommand = this.Mapper.Map(integrationCommand, integrationCommandType, applicationCommandType);
        var responseType = applicationCommandType.BaseType!.GetGenericArguments()[0];
        var operationResultType = typeof(IOperationResult<>).MakeGenericType(responseType);
        await this.Mediator.GetType().GetMethod("ExecuteAsync")!.MakeGenericMethod(operationResultType).InvokeAsync(this.Mediator, applicationCommand, new CancellationToken());
    }
}
