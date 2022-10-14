using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Synapse.Demo.Client.Rest.Services;

namespace Synapse.Demo.Client.Rest.Extensions;

/// <summary>
/// Defines extensions for <see cref="IServiceCollection"/>s
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures an HTTP client for the Synapse Management API
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="httpClientSetup">An <see cref="Action{T}"/> used to configure the <see cref="HttpClient"/> to use</param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddSynapseRestApiClient(this IServiceCollection services, Action<HttpClient> httpClientSetup)
    {
        services.AddHttpClient(typeof(RestApiClient).Name, http => httpClientSetup(http));
        services.TryAddSingleton<RestApiClient>();
        return services;
    }
}
