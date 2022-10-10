namespace Synapse.Demo.Persistence.UnitTests.Data.Factories;

internal static class EventStoreFactory
{
    internal static IEventStore Create()
    {
        ServiceCollection services = new();
        services.AddLogging();
        services.AddInMemoryEventStore();
        return services.BuildServiceProvider().GetRequiredService<IEventStore>();
    }
}
