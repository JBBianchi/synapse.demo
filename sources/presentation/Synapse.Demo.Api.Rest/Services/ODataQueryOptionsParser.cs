namespace Synapse.Demo.Api.Rest.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IODataQueryOptionsParser"/>
/// </summary>
public class ODataQueryOptionsParser
    : IODataQueryOptionsParser
{

    /// <summary>
    /// Initializes a new <see cref="ODataQueryOptionsParser"/>
    /// </summary>
    /// <param name="edmModel">The semantic description of the application's EDM model</param>
    public ODataQueryOptionsParser(IEdmModel edmModel)
    {
        this.EdmModel = edmModel;
    }

    /// <summary>
    /// Gets the semantic description of the application's EDM model
    /// </summary>
    protected IEdmModel EdmModel { get; }

    /// <inheritdoc/>
    public virtual ODataQueryOptions<TEntity> Parse<TEntity>(string? query)
        where TEntity : class, IIdentifiable
    {
        ODataQueryOptions<TEntity> queryOptions = null!;
        if (!string.IsNullOrWhiteSpace(query))
        {
            var context = new DefaultHttpContext();
            context.Request.QueryString = new($"?{query}");
            var parser = new ODataUriParser(this.EdmModel, new(string.Empty, UriKind.Relative));
            var path = parser.ParsePath();
            queryOptions = new ODataQueryOptions<TEntity>(new(this.EdmModel, typeof(TEntity), path), context.Request);
        }
        return queryOptions;
    }

}

