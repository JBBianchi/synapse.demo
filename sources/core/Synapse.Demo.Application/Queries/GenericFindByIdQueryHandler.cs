namespace Synapse.Demo.Application.Queries;

/// <summary>
/// Represents the service used to handle <see cref="GenericFindByIdQuery{TEntity, TKey}"/> instances
/// </summary>
/// <typeparam name="TEntity">The type of entity to find</typeparam>
/// <typeparam name="TKey">The type of key used to uniquely identify the entity to find</typeparam>
public class GenericFindByIdQueryHandler<TEntity, TKey>
    : QueryHandlerBase<TEntity, TKey>,
    IQueryHandler<GenericFindByIdQuery<TEntity, TKey>, TEntity>
    where TEntity : class, IIdentifiable<TKey>
    where TKey : IEquatable<TKey>
{

    /// <inheritdoc/>
    public GenericFindByIdQueryHandler(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper, IRepository<TEntity, TKey> repository)
        : base(loggerFactory, mediator, mapper, repository)
    {
    }

    /// <inheritdoc/>
    public virtual async Task<IOperationResult<TEntity>> HandleAsync(GenericFindByIdQuery<TEntity, TKey> query, CancellationToken cancellationToken = default)
    {
        var entity = await this.Repository.FindAsync(query.Id, cancellationToken);
        if (entity == null)
            throw DomainException.NullReference(typeof(TEntity), query.Id, nameof(query.Id));
        return this.Ok(entity);
    }

}