using Microsoft.Extensions.DependencyInjection;
using static System.Formats.Asn1.AsnWriter;
using DomainDevice = Synapse.Demo.Domain.Models.Device;

namespace Synapse.Demo.Application.Services;

/// <summary>
/// Represents the service used to 
/// </summary>
public class DataSeeder
    : BackgroundService
{

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Initializes a new <see cref="DataSeeder"/>
    /// </summary>
    /// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
    public DataSeeder(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw DomainException.ArgumentNull(nameof(serviceProvider));
        this.ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// Initializes the application's database
    /// </summary>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = this.ServiceProvider.CreateScope();
        var devicesRepository = scope.ServiceProvider.GetRequiredService<IRepository<DomainDevice>>();
        if (await devicesRepository.ContainsAsync("thermometer", cancellationToken))
            return;
        var devices = new List<DomainDevice>() {
            new DomainDevice("thermometer", "Temperature", "sensor", "indoor", new { temperature = "18°C" }),
            new DomainDevice("hydrometer", "Humidity", "sensor", "indoor", new { humidity = "53%" })
        };
        foreach(var device in devices)
        {
            await devicesRepository.AddAsync(device, cancellationToken);
        }
        await devicesRepository.SaveChangesAsync(cancellationToken);
    }
}
