using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Neuroglia.Eventing.Services;

namespace Synapse.Demo.Persistence.IntegrationTests.Data.Factories;

internal static class RepositoryFactory
{
    internal static async Task<T> Create<T>()
        where T : class, IRepository
    {
        var configuration = new DemoApplicationOptions()
        {
            CloudEventsSource = "https://demo.synpase.com",
            CloudEventBroker = "https://webhook.site/2938e77a-0508-4d98-8d75-3830262437b3",
            SchemaRegistry = "https://schema-registry.synapse.com"
        };
        ServiceCollection services = new();
        services.TryAddSingleton(Options.Create(configuration));
        services.AddLogging();
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddPersistence();
        var serviceProvider = services.BuildServiceProvider();
        var cloudEventBus = serviceProvider.GetRequiredService<CloudEventBus>();
        await cloudEventBus.StartAsync(new CancellationToken());
        await Task.Delay(1);
        await cloudEventBus.StopAsync(new CancellationToken());
        return serviceProvider.GetRequiredService<T>();
    }
}
