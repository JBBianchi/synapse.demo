namespace Synapse.Demo.Application.Queries;

/// <summary>
/// Represents the <see cref="IQuery"/> used to filter the entities of the specified type
/// </summary>
/// <typeparam name="TEntity">The type of <see cref="IEntity"/>The type of entities to query</typeparam>
public class GenericFilterQuery<TEntity>
    : Query<List<TEntity>>
    where TEntity : class, IIdentifiable
{

    /// <summary>
    /// Gets the <see cref="ODataQueryOptions"/> used to filter the entities
    /// </summary>
    public ODataQueryOptions<TEntity> Options { get; protected set; }

    /// <summary>
    /// Initializes a new <see cref="GenericFilterQuery{TEntity}"/>
    /// </summary>
    protected GenericFilterQuery()
    {
        this.Options = null!;
    }

    /// <summary>
    /// Initializes a new <see cref="GenericFilterQuery{TEntity}"/>
    /// </summary>
    /// <param name="options">The <see cref="ODataQueryOptions"/> used to filter the entities</param>
    public GenericFilterQuery(ODataQueryOptions<TEntity> options)
    {
        this.Options = options;
    }

}
