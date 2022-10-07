namespace Synapse.Demo.Persistence.Extensions.DependencyInjection;


/// <summary>
/// Extension methods for setting up the persitence services in an <see cref="IServiceCollection" />.
/// </summary>
public static class PersistenceServiceCollectionExtensions
{
    /// <summary>
    /// Adds the persitence services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        AddWriteRepo(ESinMemory);
        AddReadRepo(WhichEverRepoInMemory);
        return services;
    }
}
