namespace Synapse.Demo.Application.UnitTests.Data.Factories;

internal static class MapperFactory
{
    internal static IMapper Create()
    {
        ServiceCollection services = new();
        services.AddApplicationMapper();
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }
}
