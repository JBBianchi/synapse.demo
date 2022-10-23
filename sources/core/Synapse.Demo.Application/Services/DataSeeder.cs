using DomainDevice = Synapse.Demo.Domain.Models.Device;

namespace Synapse.Demo.Application.Services;

/// <summary>
/// Represents the service used to seed data
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
        if (await devicesRepository.ContainsAsync(ApplicationConstants.DeviceIds.Thermometer, cancellationToken))
            return;
        var devices = new List<DomainDevice>() {
            new DomainDevice(ApplicationConstants.DeviceIds.Thermometer, "Temperature", "sensor.thermometer", "indoor", new { temperature = 16 /*, desired = 19*/ }),
            new DomainDevice(ApplicationConstants.DeviceIds.Hydrometer, "Humidity", "sensor.hydrometer", "indoor", new { humidity = 53 }),
            new DomainDevice(ApplicationConstants.DeviceIds.Heater, "Heater", "equipment.heater", "indoor.cellar", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.AirConditioning, "A/C", "equipment.air-conditioning", "indoor.living", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.HallwayLights, "Hallway lights", "switch.light", "indoor.hallway", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.LivingLights, "Living lights", "switch.light", "indoor.living", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.HallwayMotionSensor, "Hallway motion", "sensor.motion", "indoor.hallway", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.LivingMotionSensor, "Living motion", "sensor.motion", "indoor.living", new { on = false })
        };
        foreach(var device in devices)
        {
            await devicesRepository.AddAsync(device, cancellationToken);
        }
        await devicesRepository.SaveChangesAsync(cancellationToken);
    }
}
