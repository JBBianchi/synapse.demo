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
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        if (services == null) throw DomainException.ArgumentNull(nameof(services));
        services.AddApplicationMediator();
        services.AddApplicationMapper();
        return services;
    }

    /// <summary>
    /// Adds the application <see cref="Mediator"/> configuration
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationMediator(this IServiceCollection services)
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
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationMapper(this IServiceCollection services)
    {
        Assembly applicationAssembly = typeof(ApplicationServiceCollectionExtensions).Assembly;
        services.AddMapper(applicationAssembly);
        return services;
    }
}
