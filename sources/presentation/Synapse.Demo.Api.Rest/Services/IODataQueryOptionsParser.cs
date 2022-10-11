namespace Synapse.Demo.Api.Rest.Services;

/// <summary>
/// Defines the fundamentals of a service used to parse <see cref="ODataQueryOptions"/>
/// </summary>
public interface IODataQueryOptionsParser
{

    /// <summary>
    /// Parses the specified ODATA query string into a new <see cref="ODataQueryOptions{TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to query</typeparam>
    /// <param name="query">The ODATA query string to parse</param>
    /// <returns>A new <see cref="ODataQueryOptions{TEntity}"/></returns>
    ODataQueryOptions<TEntity> Parse<TEntity>(string? query)
        where TEntity : class, IIdentifiable;

}

