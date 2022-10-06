namespace Synapse.Demo.Application.Behaviors;

// TODO: Write tests
/// <summary>
/// A <see cref="IMiddleware<TRequest, TResult>"/> used to log a <see cref="TRequest"/>s
/// </summary>
/// <typeparam name="TRequest">The incoming <see cref="TRequest"/></typeparam>
/// <typeparam name="TResult">The outgoing <see cref="TResult"/></typeparam>
internal class RequestLogger<TRequest, TResult>
    : IMiddleware<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : IOperationResult
{
    /// <summary>
    /// Gets the <see cref="ILogger/>
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Constructs a new <see cref="RequestLogger<TRequest, TResult>"/>
    /// </summary>
    /// <param name="logger">The <see cref="ILogger/></param>
    public RequestLogger(ILogger<RequestLogger<TRequest, TResult>> logger)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Logs the request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult> HandleAsync(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken = default)
    {
        var requestName = typeof(TRequest).Name;
        this._logger.LogTrace($"Processing request '{requestName}': {request}");
        var reponse = (await next());
        this._logger.LogTrace($"Processed request '{requestName}'");
        return reponse;
    }
}
