namespace Synapse.Demo.Application.Commands;

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
        if (loggerFactory == null) throw DomainException.ArgumentNull(nameof(loggerFactory));
        if (mediator == null) throw DomainException.ArgumentNull(nameof(mediator));
        if (mapper == null) throw DomainException.ArgumentNull(nameof(mapper));
        this.Logger = loggerFactory.CreateLogger(this.GetType());
        this.Mediator = mediator;
        this.Mapper = mapper;
    }

}