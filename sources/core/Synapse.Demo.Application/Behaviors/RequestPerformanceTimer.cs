using System.Diagnostics;

namespace Synapse.Demo.Application.Behaviors;

// TODO: Write tests
/// <summary>
/// A <see cref="IMiddleware<TRequest, TResult>"/> used to time a <see cref="TRequest"/>s
/// </summary>
/// <typeparam name="TRequest">The incoming <see cref="TRequest"/></typeparam>
/// <typeparam name="TResult">The outgoing <see cref="TResult"/></typeparam>
internal class RequestPerformanceTimer<TRequest, TResult>
    : IMiddleware<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : IOperationResult
{
    /// <summary>
    /// Gets a <see cref="ILogger/>
    /// </summary>
    protected ILogger Logger { get; init; }

    /// <summary>
    /// Gets a <see cref="System.Diagnostics.Stopwatch/>
    /// </summary>
    protected Stopwatch Stopwatch { get; init; }

    /// <summary>
    /// Initializes a new <see cref="RequestPerformanceTimer<TRequest, TResult>"/>
    /// </summary>
    /// <param name="logger"></param>
    public RequestPerformanceTimer(ILogger<RequestPerformanceTimer<TRequest, TResult>> logger)
    {
        this.Logger = logger;
        this.Stopwatch = new();
    }

    /// <summary>
    /// Times the request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult> HandleAsync(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken = default)
    {
        this.Stopwatch.Start();
        var reponse = (await next());
        this.Stopwatch.Stop();
        if (this.Stopwatch.ElapsedMilliseconds > 300)
        {
            var requestName = typeof(TRequest).Name;
            this.Logger.LogWarning($"The request '{requestName}' took more than {this.Stopwatch.ElapsedMilliseconds}ms to be processed.");
        }
        return reponse;
    }
}
