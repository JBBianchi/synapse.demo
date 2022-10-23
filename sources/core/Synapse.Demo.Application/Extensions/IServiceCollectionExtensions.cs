namespace Synapse.Demo.Application.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up the application services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Adds the application services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The current <see cref="IConfiguration"/></param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddDemoApplication(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        services.AddDemoApplication(configuration, builder => { });
        return services;
    }

    /// <summary>
    /// Adds the application services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The current <see cref="IConfiguration"/></param>
    /// <param name="setup">An <see cref="Action{T}"/> used to configure the app</param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddDemoApplication(this IServiceCollection services, IConfiguration configuration, Action<IDemoApplicationBuilder> setup)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        var options = new DemoApplicationOptions();
        configuration.Bind(options);
        services.Configure<DemoApplicationOptions>(configuration);
        setup(new DemoApplicationBuilder(services, configuration, options));
        return services;
    }

    /// <summary>
    /// Adds the application <see cref="Mediator"/> configuration
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddDemoApplicationMediator(this IServiceCollection services)
    {
        Assembly applicationAssembly = typeof(ApplicationServiceCollectionExtensions).Assembly;
        services.AddMediator(options =>
        {
            options.ScanAssembly(applicationAssembly);
            options.UseDefaultPipelineBehavior(typeof(RequestPerformanceTimer<,>));
            options.UseDefaultPipelineBehavior(typeof(RequestLogger<,>));
            options.UseDefaultPipelineBehavior(typeof(DomainExceptionHandlingMiddleware<,>));
            options.UseDefaultPipelineBehavior(typeof(FluentValidationMiddleware<,>));
        });
        return services;
    }

    /// <summary>
    /// Adds the application <see cref="Mapper"/> configuration
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddDemoApplicationMapper(this IServiceCollection services)
    {
        Assembly applicationAssembly = typeof(ApplicationServiceCollectionExtensions).Assembly;
        services.AddMapper(applicationAssembly);
        Assembly integrationAssembly = typeof(Integration.ModelDto).Assembly;
        services.AddMapper(integrationAssembly);
        return services;
    }

    /// <summary>
    /// Adds the services required to handle generic queries.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="serviceLifetime">The lifetime of the services to add. Defaults to <see cref="ServiceLifetime.Scoped"/></param>
    /// <returns>The configured <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    internal static IServiceCollection AddGenericQueryHandlers(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        foreach (Type queryableType in TypeCacheUtil.FindFilteredTypes("integration:queryable-types", t => t.TryGetCustomAttribute<QueryableAttribute>(out _), typeof(QueryableAttribute).Assembly))
        {
            var keyType = queryableType.GetGenericType(typeof(IIdentifiable<>)).GetGenericArguments().First();
            var queryType = typeof(GenericFindByIdQuery<,>).MakeGenericType(queryableType, keyType);
            var resultType = typeof(IOperationResult<>).MakeGenericType(queryableType);
            var handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(queryType, resultType);
            var handlerImplementationType = typeof(GenericFindByIdQueryHandler<,>).MakeGenericType(queryableType, keyType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(IMiddleware<,>).MakeGenericType(queryType, resultType), typeof(DomainExceptionHandlingMiddleware<,>).MakeGenericType(queryType, resultType), serviceLifetime));

            queryType = typeof(GenericListQuery<>).MakeGenericType(queryableType);
            resultType = typeof(IOperationResult<>).MakeGenericType(typeof(IQueryable<>).MakeGenericType(queryableType));
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(queryType, resultType);
            handlerImplementationType = typeof(GenericListQueryHandler<>).MakeGenericType(queryableType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(IMiddleware<,>).MakeGenericType(queryType, resultType), typeof(DomainExceptionHandlingMiddleware<,>).MakeGenericType(queryType, resultType), serviceLifetime));

            queryType = typeof(GenericFilterQuery<>).MakeGenericType(queryableType);
            resultType = typeof(IOperationResult<>).MakeGenericType(typeof(List<>).MakeGenericType(queryableType));
            handlerServiceType = typeof(IRequestHandler<,>).MakeGenericType(queryType, resultType);
            handlerImplementationType = typeof(GenericFilterQueryHandler<>).MakeGenericType(queryableType);
            services.Add(new ServiceDescriptor(handlerServiceType, handlerImplementationType, serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(IMiddleware<,>).MakeGenericType(queryType, resultType), typeof(DomainExceptionHandlingMiddleware<,>).MakeGenericType(queryType, resultType), serviceLifetime));
        }
        return services;
    }
}
