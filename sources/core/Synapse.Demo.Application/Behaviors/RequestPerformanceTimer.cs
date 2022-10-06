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
    private readonly ILogger _logger;

    /// <summary>
    /// Gets a <see cref="Stopwatch/>
    /// </summary>
    private readonly Stopwatch _stopwatch;

    /// <summary>
    /// Constructs a new <see cref="RequestPerformanceTimer<TRequest, TResult>"/>
    /// </summary>
    /// <param name="logger"></param>
    public RequestPerformanceTimer(ILogger<RequestPerformanceTimer<TRequest, TResult>> logger)
    {
        this._logger = logger;
        this._stopwatch = new();
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
        this._stopwatch.Start();
        var reponse = (await next());
        this._stopwatch.Stop();
        if (this._stopwatch.ElapsedMilliseconds > 300)
        {
            var requestName = typeof(TRequest).Name;
            this._logger.LogWarning($"The request '{requestName}' took more than {this._stopwatch.ElapsedMilliseconds}ms to be processed.");
        }
        return reponse;
    }
}
