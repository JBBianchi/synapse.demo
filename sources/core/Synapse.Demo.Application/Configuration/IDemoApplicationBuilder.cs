using Microsoft.Extensions.Configuration;

namespace Synapse.Demo.Application.Extensions.DependencyInjection;

/// <summary>
/// An interface for configuring application services.
/// </summary>
public interface IDemoApplicationBuilder
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> where MVC services are configured.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Gets the <see cref="IConfiguration"/> to use
    /// </summary>
    IConfiguration Configuration { get; }
}
