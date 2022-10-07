namespace Synapse.Demo.Application.UnitTests.Data.Factories;

internal static class MapperFactory
{
    internal static IMapper Create(Action<IServiceCollection>? serviceConfiguration = null)
    {
        ServiceCollection services = new();
        services.AddApplicationMapper();
        if (serviceConfiguration != null) serviceConfiguration(services);
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }
}
