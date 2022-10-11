namespace Synapse.Demo.Infrastructure.Extensions.DependencyInjection;


/// <summary>
/// Extension methods for setting up the infrastructure services in an <see cref="IServiceCollection" />.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Adds the infrastructure services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddDemoInfrastructure(this IServiceCollection services, IDemoApplicationOptions applicationOptions)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        services.AddSingleton<CloudEventFormatter, JsonEventFormatter>();
        services.AddCloudEventBus(builder =>
        {
            builder.WithBrokerUri(new (applicationOptions.CloudEventBroker));
        });
        return services;
    }
}
