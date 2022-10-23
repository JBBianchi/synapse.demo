using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Neuroglia.Eventing.Services;

namespace Synapse.Demo.Persistence.IntegrationTests.Data.Factories;

internal static class RepositoryFactory
{
    internal static async Task<T> Create<T>()
        where T : class, IRepository
    {
        var optionsDictionnary = new Dictionary<string, string>()
        {
            { "CloudEventsSource", "https://demo.synpase.com" },
            { "CloudEventBroker", "https://webhook.site/60a98df9-2b4b-47e5-a94e-f45b437424c6" },
            { "SchemaRegistry", "https://schema-registry.synapse.com" }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(optionsDictionnary)
            .Build();
        ServiceCollection services = new();
        services.AddLogging();
        services.AddDemoApplication(configuration, demoBuilder =>
        {
            demoBuilder.AddInfrastructure();
            demoBuilder.AddPersistence();
        });
        var serviceProvider = services.BuildServiceProvider();
        var cloudEventBus = serviceProvider.GetRequiredService<CloudEventBus>();
        await cloudEventBus.StartAsync(new CancellationToken());
        await Task.Delay(1);
        await cloudEventBus.StopAsync(new CancellationToken());
        return serviceProvider.GetRequiredService<T>();
    }
}
