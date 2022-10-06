namespace Synapse.Demo.Application.Commands;

/// <summary>
/// Represents the base class for all of the application's <see cref="ICommandHandler"/> implementations
/// </summary>
internal abstract class CommandHandlerBase
{

    /// <summary>
    /// Initializes a new <see cref="CommandHandlerBase"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    protected CommandHandlerBase(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper)
    {
        this._logger = loggerFactory.CreateLogger(this.GetType());
        this._mediator = mediator;
        this._mapper = mapper;
    }

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected readonly ILogger _logger;

    /// <summary>
    /// Gets the service used to mediate calls
    /// </summary>
    protected readonly IMediator _mediator;

    /// <summary>
    /// Gets the service used to map objects
    /// </summary>
    protected readonly IMapper _mapper;

}
