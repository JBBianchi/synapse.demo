namespace Synapse.Demo.Application.CommandHandlers;

/// <summary>
/// Represents the base class for all of the application's <see cref="ICommandHandler"/> implementations
/// </summary>
internal abstract class CommandHandlerBase
{

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; init; }

    /// <summary>
    /// Gets the service used to mediate calls
    /// </summary>
    protected IMediator Mediator { get; init; }

    /// <summary>
    /// Gets the service used to map objects
    /// </summary>
    protected IMapper Mapper { get; init; }

    /// <summary>
    /// Initializes a new <see cref="CommandHandlerBase"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    protected CommandHandlerBase(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper)
    {
        this.Logger = loggerFactory.CreateLogger(this.GetType());
        this.Mediator = mediator;
        this.Mapper = mapper;
    }

}

// TODO: currently, commandles handlers handle "integration" commands, they should handle "application" commands