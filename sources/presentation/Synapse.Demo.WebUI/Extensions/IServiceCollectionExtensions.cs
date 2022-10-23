namespace Synapse.Demo.WebUI.Extensions;
/// <summary>
/// Extension methods for setting up the web UI services in an <see cref="IServiceCollection" />.
/// </summary>
public static class WebUIServiceCollectionExtensions
{
    /// <summary>
    /// Adds the web UI services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="baseAddress">The web app base address</param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddWebUI(this IServiceCollection services, string baseAddress)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        services.AddWebUIClients(baseAddress)
            .AddWebUIMapper()
            .AddWebUIFlux()
            .AddWebUIServices();
        return services;
    }

    /// <summary>
    /// Adds the web UI rest & web socket client's configuration
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="baseAddress">The web app base address</param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddWebUIClients(this IServiceCollection services, string baseAddress)
    {
        var baseAddressUri = new Uri(baseAddress);
        services.AddScoped(sp => new HttpClient { BaseAddress = baseAddressUri });
        services.AddRestApiClient(http => http.BaseAddress = baseAddressUri);
        services.AddNewtonsoftJsonSerializer(options => options.ConfigureSerializerSettings());
        services.AddSingleton(provider =>
            new HubConnectionBuilder()
                .WithUrl($"{baseAddress}api/ws")
                .WithAutomaticReconnect()
                .AddNewtonsoftJsonProtocol(options => {
                    options.PayloadSerializerSettings = new JsonSerializerSettings().ConfigureSerializerSettings();
                })
                .Build()
        );
        return services;
    }

    /// <summary>
    /// Adds the web UI <see cref="Mapper"/> configuration
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddWebUIMapper(this IServiceCollection services)
    {
        Assembly integrationAssembly = typeof(Integration.ModelDto).Assembly;
        services.AddMapper(integrationAssembly);
        return services;
    }

    /// <summary>
    /// Adds the web UI flux configuration
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddWebUIFlux(this IServiceCollection services)
    {
        services.AddFlux(flux => flux.ScanMarkupTypeAssembly<App>());
        return services;
    }

    /// <summary>
    /// Adds the web UI services
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddSingleton<IKnobManager, KnobManager>();
        return services;
    }
}
