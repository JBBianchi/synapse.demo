namespace Synapse.Demo.Application.Queries;

/// <summary>
/// Represents an <see cref="IQuery"/> used to get an entity by id
/// </summary>
/// <typeparam name="TEntity">The type of entity to query</typeparam>
/// <typeparam name="TKey">The type of id used to uniquely identify the entity to get</typeparam>
public class GenericFindByIdQuery<TEntity, TKey>
    : Query<TEntity>
    where TEntity : class, IIdentifiable<TKey>
    where TKey : IEquatable<TKey>
{

    /// <summary>
    /// Gets the id of the entity to find
    /// </summary>
    public virtual TKey Id { get; }

    /// <summary>
    /// Initializes a new <see cref="GenericFindByIdQuery{TEntity, TKey}"/>
    /// </summary>
    protected GenericFindByIdQuery()
    {
        this.Id = default!;
    }

    /// <summary>
    /// Initializes a new <see cref="GenericFindByIdQuery{TEntity, TKey}"/>
    /// </summary>
    /// <param name="id">The id of the entity to find</param>
    public GenericFindByIdQuery(TKey id)
    {
        this.Id = id;
    }

}
