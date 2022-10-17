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
            { "CloudEventBroker", "https://webhook.site/23fe6664-6e3f-4ed2-ba37-eb1472d4852b" },
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
