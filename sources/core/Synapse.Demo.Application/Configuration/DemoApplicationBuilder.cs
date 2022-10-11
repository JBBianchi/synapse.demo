using Microsoft.Extensions.Configuration;

namespace Synapse.Demo.Application.Configuration;

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
    /// Gets the <see cref="DemoApplicationOptions"/>
    /// </summary>
    public IDemoApplicationOptions Options { get; private set; }

    /// <summary>
    /// Initializes a new <see cref="DemoApplicationBuilder"/> instance.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The current <see cref="IConfiguration"/></param>
    public DemoApplicationBuilder(IServiceCollection services, IConfiguration configuration, DemoApplicationOptions options)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        if (configuration == null) throw DomainException.ArgumentNull(nameof(configuration));
        if (options == null) throw DomainException.ArgumentNull(nameof(options));
        this.Services = services;
        this.Configuration = configuration;
        this.Options = options;
        this.AddApplication();
    }
}
