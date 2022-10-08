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

        return services;
    }

    /// <summary>
    /// Adds and configures the <see cref="InMemoryEventStore"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddInMemoryEventStore(this IServiceCollection services)
    {
        services.TryAddSingleton<InMemoryEventStore>();
        services.TryAddSingleton<IEventStore>(provider => provider.GetRequiredService<InMemoryEventStore>());
        return services;
    }

    /// <summary>
    /// Adds and configures the <see cref="InMemoryReadRepository{TEntity, TKey}"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the <see cref="InMemoryReadRepository{TEntity, TKey}"/> to add. Defaults to <see cref="ServiceLifetime.Scoped"/></param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddInMemoryReadRepository<TEntity, TKey>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TEntity : class, IIdentifiable<TKey>
        where TKey : IEquatable<TKey>
    {
        return services.AddInMemoryReadRepository(typeof(TEntity), typeof(TKey), lifetime);
    }

    /// <summary>
    /// Adds and configures the <see cref="InMemoryReadRepository{TEntity, TKey}"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="entityType">The type of entity to store</param>
    /// <param name="keyType">The type of key used to uniquely identify entities to store</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the <see cref="InMemoryReadRepository{TEntity, TKey}"/> to add. Defaults to <see cref="ServiceLifetime.Scoped"/></param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    private static IServiceCollection AddInMemoryReadRepository(this IServiceCollection services, Type entityType, Type keyType, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        if (entityType == null)
            throw new ArgumentNullException(nameof(entityType));
        if (!typeof(IIdentifiable).IsAssignableFrom(entityType))
            throw new ArgumentException($"Type '{entityType.Name}' is not an implementation of the '{nameof(IIdentifiable)}' interface", nameof(entityType));
        var identifiableType = entityType.GetGenericType(typeof(IIdentifiable<>));
        var expectedKeyType = identifiableType.GetGenericArguments()[0];
        if (keyType == null)
            throw new ArgumentNullException(nameof(keyType));
        if (!expectedKeyType.IsAssignableFrom(keyType))
            throw new ArgumentException($"Type '{entityType.Name}' expects a key of type '{expectedKeyType.Name}'", nameof(keyType));
        Type implementationType = typeof(InMemoryReadRepository<,>).MakeGenericType(entityType, keyType);
        services.TryAdd(new ServiceDescriptor(implementationType, implementationType, lifetime));
        services.TryAdd(new ServiceDescriptor(typeof(IRepository<,>).MakeGenericType(entityType, keyType), provider => provider.GetRequiredService(implementationType), lifetime));
        services.TryAdd(new ServiceDescriptor(typeof(IRepository<>).MakeGenericType(entityType), provider => provider.GetRequiredService(implementationType), lifetime));
        return services;
    }
}
