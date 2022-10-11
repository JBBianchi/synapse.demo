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
    public static IServiceCollection AddDemoPersistence(this IServiceCollection services)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        List<Type> writeModelTypes = TypeCacheUtil.FindFilteredTypes("domain:aggregates", t => t.IsClass && !t.IsAbstract && typeof(IAggregateRoot).IsAssignableFrom(t), typeof(Domain.Models.Device).Assembly).ToList();
        List<Type> readModelTypes = writeModelTypes
            .Where(t => t.TryGetCustomAttribute<DataTransferObjectTypeAttribute>(out _))
            .Select(t => t.GetCustomAttribute<DataTransferObjectTypeAttribute>()!.Type)
            .ToList();
        services.AddDemoInMemoryEventStore();
        services.AddDemoRepositories(writeModelTypes, typeof(EventSourcingRepository<,>));
        services.AddDemoRepositories(readModelTypes, typeof(InMemoryDbRepository<,>));
        services.AddDemoRepository(typeof(CloudEventOutboxEntry), typeof(InMemoryDbRepository<,>), ServiceLifetime.Singleton);
        return services;
    }

    /// <summary>
    /// Adds and configures the <see cref="InMemoryEventStore"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddDemoInMemoryEventStore(this IServiceCollection services)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        services.TryAddSingleton<IAggregatorFactory, AggregatorFactory>();
        services.TryAddSingleton<InMemoryEventStore>();
        services.TryAddSingleton<IEventStore>(provider => provider.GetRequiredService<InMemoryEventStore>());
        return services;
    }

    /// <summary>
    /// Adds and configures the <see cref="IRepository{TEntity, TKey}"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="entityType">The type of entity to store</param>
    /// <param name="repositoryType">The type of <see cref="IRepository{TEntity, TKey}"/> implementation used for the provided entityType</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> of the <see cref="IRepository{TEntity, TKey}"/> to add. Defaults to <see cref="ServiceLifetime.Scoped"/></param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddDemoRepository(this IServiceCollection services, Type entityType, Type repositoryType, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        if (entityType == null) throw DomainException.ArgumentNull(nameof(entityType));
        if (repositoryType == null) throw DomainException.ArgumentNull(nameof(repositoryType));
        Type keyType = entityType.GetGenericType(typeof(IIdentifiable<>)).GetGenericArguments()[0];
        if (keyType == null) throw DomainException.NullReference(nameof(services));
        Type implementationType = repositoryType.MakeGenericType(entityType, keyType);
        if (implementationType == null) throw DomainException.NullReference(nameof(implementationType));
        services.Add(new(implementationType, implementationType, serviceLifetime));
        services.Add(new(typeof(IRepository<>).MakeGenericType(entityType), provider => provider.GetRequiredService(implementationType), serviceLifetime));
        services.Add(new(typeof(IRepository<,>).MakeGenericType(entityType, keyType), provider => provider.GetRequiredService(implementationType), serviceLifetime));
        return services;
    }


    /// <summary>
    /// Adds and configures the <see cref="IRepository{TEntity, TKey}"/>s
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="entityTypes">The types of entity to store</param>
    /// <param name="repositoryType">The type of <see cref="IRepository{TEntity, TKey}"/> implementation used for the provided entity types</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> of the <see cref="IRepository{TEntity, TKey}"/> to add. Defaults to <see cref="ServiceLifetime.Scoped"/></param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddDemoRepositories(this IServiceCollection services, IEnumerable<Type> entityTypes, Type repositoryType, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        if (entityTypes == null || !entityTypes.Any()) throw DomainException.ArgumentNull(nameof(entityTypes));
        if (repositoryType == null) throw DomainException.ArgumentNull(nameof(repositoryType));
        foreach (Type entityType in entityTypes)
        {
            services.AddDemoRepository(entityType, repositoryType, serviceLifetime);
        }
        return services;
    }
}
