namespace Synapse.Demo.Application.Queries;

/// <summary>
/// Represents the service used to handle <see cref="GenericFilterQuery{TEntity}"/> instances
/// </summary>
/// <typeparam name="TEntity">The type of entity to filter</typeparam>
public class GenericFilterQueryHandler<TEntity>
    : QueryHandlerBase<TEntity>,
    IQueryHandler<GenericFilterQuery<TEntity>, List<TEntity>>
    where TEntity : class, IIdentifiable
{

    /// <summary>
    /// Gets the service used to bind ODATA searches
    /// </summary>
    protected ISearchBinder SearchBinder { get; }

    /// <summary>
    /// Gets the current <see cref="IEdmModel"/>
    /// </summary>
    protected IEdmModel EdmModel { get; }

    /// <inheritdoc/>
    public GenericFilterQueryHandler(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper, IRepository<TEntity> repository, ISearchBinder searchBinder, IEdmModel edmModel)
        : base(loggerFactory, mediator, mapper, repository)
    {
        this.SearchBinder = searchBinder;
        this.EdmModel = edmModel;
    }

    /// <inheritdoc/>
    public virtual async Task<IOperationResult<List<TEntity>>> HandleAsync(GenericFilterQuery<TEntity> query, CancellationToken cancellationToken = default)
    {
        var toFilter = (await this.Repository.ToListAsync(cancellationToken)).AsQueryable();
        if (query.Options?.Search != null)
        {
            var searchExpression = (Expression<Func<TEntity, bool>>)this.SearchBinder.BindSearch(query.Options.Search.SearchClause, new(this.EdmModel, new(), typeof(TEntity)));
            toFilter = toFilter.Where(searchExpression);
        }
        var filtered = query.Options?.ApplyTo(toFilter);
        if (filtered == null)
            filtered = toFilter;
        return this.Ok(filtered.OfType<TEntity>().ToList());
    }

}