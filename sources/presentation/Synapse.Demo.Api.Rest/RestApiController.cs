namespace Synapse.Demo.Api.Rest;

/// <summary>
/// A base class for a RESTful api controller
/// </summary>
public abstract class RestApiController
    : ControllerBase
{
    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets the service used to mediate calls
    /// </summary>
    protected IMediator Mediator { get; }

    /// <summary>
    /// Gets the service used to map objects
    /// </summary>
    protected IMapper Mapper { get; }

    /// <summary>
    /// Initializes a new <see cref="RestApiController"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    protected RestApiController(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper)
    {
        this.Logger = loggerFactory.CreateLogger(this.GetType());
        this.Mediator = mediator;
        this.Mapper = mapper;
    }
}
