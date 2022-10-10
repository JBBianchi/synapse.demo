using Microsoft.Extensions.Configuration;

namespace Synapse.Demo.Application.Extensions.DependencyInjection;

/// <summary>
/// Allows a fine grained configuration of Application services
/// </summary>
internal class DemoApplicationBuilder
    : IDemoApplicationBuilder
{

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <inheritdoc />
    public IConfiguration Configuration { get; private set; }

    /// <summary>
    /// Initializes a new <see cref="DemoApplicationBuilder"/> instance.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    public DemoApplicationBuilder(IServiceCollection services)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        this.Services = services;
        this.Configuration = null!;
    }
}
