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
        await Task.Delay(5000);
        var devices = new List<DomainDevice>() {
            new DomainDevice("thermometer", "Temperature", "sensor.thermometer", "indoor", new { temperature = "18°C" }),
            new DomainDevice("hydrometer", "Humidity", "sensor.hydrometer", "indoor", new { humidity = "53%" }),
            new DomainDevice("lights-hallway", "Hallway lights", "switch.light", "indoor.hallway", new { on = false }),
            new DomainDevice("lights-living", "Living lights", "switch.light", "indoor.living", new { on = true }),
            new DomainDevice("heater", "Heater", "equipment.heater", "indoor.cellar", new { on = true }),
            new DomainDevice("air-conditioning", "A/C", "equipment.air-conditioning", "indoor.living", new { on = false })
        };
        foreach(var device in devices)
        {
            await devicesRepository.AddAsync(device, cancellationToken);
        }
        await devicesRepository.SaveChangesAsync(cancellationToken);
    }
}
