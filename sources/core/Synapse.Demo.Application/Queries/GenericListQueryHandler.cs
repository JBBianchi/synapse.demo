namespace Synapse.Demo.Application.Queries;

/// <summary>
/// Represents the service used to handle <see cref="GenericListQuery{TEntity}"/> instances
/// </summary>
/// <typeparam name="TEntity">The type of entity to query</typeparam>
public class GenericListQueryHandler<TEntity>
    : QueryHandlerBase<TEntity>,
    IQueryHandler<GenericListQuery<TEntity>, IQueryable<TEntity>>
    where TEntity : class, IIdentifiable
{

    /// <inheritdoc/>
    public GenericListQueryHandler(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper, IRepository<TEntity> repository)
        : base(loggerFactory, mediator, mapper, repository)
    {}

    /// <inheritdoc/>
    public virtual async Task<IOperationResult<IQueryable<TEntity>>> HandleAsync(GenericListQuery<TEntity> query, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(this.Ok(this.Repository.AsQueryable()));
    }

}