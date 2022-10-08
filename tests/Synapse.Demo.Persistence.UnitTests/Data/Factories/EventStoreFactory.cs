namespace Synapse.Demo.Persistence.UnitTests.Data.Factories;

internal static class EventStoreFactory
{
    internal static IEventStore Create(Action<IServiceCollection>? serviceConfiguration = null)
    {
        ServiceCollection services = new();
        services.AddLogging();
        services.AddInMemoryEventStore();
        if (serviceConfiguration != null) serviceConfiguration(services);
        return services.BuildServiceProvider().GetRequiredService<IEventStore>();
    }
}
