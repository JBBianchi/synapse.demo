namespace Synapse.Demo.Application.Queries;

/// <summary>
/// Represents the <see cref="IQuery"/> used to get an <see cref="IQueryable"/> of the entities of the specified type
/// </summary>
/// <typeparam name="TEntity">The type of <see cref="IEntity"/>The type of entities to query</typeparam>
public class GenericListQuery<TEntity>
    : Query<IQueryable<TEntity>>
    where TEntity : class, IIdentifiable
{}
