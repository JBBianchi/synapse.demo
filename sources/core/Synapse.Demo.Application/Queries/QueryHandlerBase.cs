namespace Synapse.Demo.Application.Queries;

/// <summary>
/// Represents the base class for all <see cref="IQueryHandler"/> implementations
/// </summary>
public abstract class QueryHandlerBase
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
    /// Initializes a new <see cref="QueryHandlerBase"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    protected QueryHandlerBase(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper)
    {
        if (loggerFactory == null) throw DomainException.ArgumentNull(nameof(loggerFactory));
        if (mediator == null) throw DomainException.ArgumentNull(nameof(mediator));
        if (mapper == null) throw DomainException.ArgumentNull(nameof(mapper));
        this.Logger = loggerFactory.CreateLogger(this.GetType());
        this.Mediator = mediator;
        this.Mapper = mapper;
    }
}

/// <summary>
/// Represents the base class for all <see cref="IQueryHandler"/> implementations
/// </summary>
/// <typeparam name="TEntity">The type of entity to query</typeparam>
public abstract class QueryHandlerBase<TEntity>
    : QueryHandlerBase
    where TEntity : class, IIdentifiable
{
    /// <summary>
    /// Gets the <see cref="IRepository"/> used to manage the entities to query
    /// </summary>
    protected IRepository<TEntity> Repository { get; }

    /// <summary>
    /// Initializes a new <see cref="QueryHandlerBase"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    /// <param name="repository">The <see cref="IRepository"/> used to manage the entities to query</param>
    protected QueryHandlerBase(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper, IRepository<TEntity> repository)
        : base(loggerFactory, mediator, mapper)
    {
        this.Repository = repository;
    }
}

/// <summary>
/// Represents the base class for all <see cref="IQueryHandler"/> implementations
/// </summary>
/// <typeparam name="TEntity">The type of entity to query</typeparam>
/// <typeparam name="TKey">The type of key used to uniquely identify the entities to query</typeparam>
public abstract class QueryHandlerBase<TEntity, TKey>
    : QueryHandlerBase<TEntity>
    where TEntity : class, IIdentifiable<TKey>
    where TKey : IEquatable<TKey>
{

    /// <summary>
    /// Gets the <see cref="IRepository"/> used to manage the entities to query
    /// </summary>
    protected new IRepository<TEntity, TKey> Repository => (IRepository<TEntity, TKey>)base.Repository;

    /// <summary>
    /// Initializes a new <see cref="QueryHandlerBase"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    /// <param name="repository">The <see cref="IRepository"/> used to manage the entities to query</param>
    protected QueryHandlerBase(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper, IRepository<TEntity, TKey> repository)
        : base(loggerFactory, mediator, mapper, repository)
    {

    }
}