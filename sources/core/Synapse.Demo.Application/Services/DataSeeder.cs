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
            new DomainDevice(ApplicationConstants.DeviceIds.Thermometer, "Temperature", ApplicationConstants.DeviceTypes.ThermometerSensor, "indoor", new { temperature = 16 /*, desired = 19*/ }),
            new DomainDevice(ApplicationConstants.DeviceIds.Hydrometer, "Humidity", ApplicationConstants.DeviceTypes.HydrometerSensor, "indoor", new { humidity = 53 }),
            new DomainDevice(ApplicationConstants.DeviceIds.Heater, "Heater", ApplicationConstants.DeviceTypes.HeaterEquipment, "indoor.cellar", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.AirConditioning, "A/C", ApplicationConstants.DeviceTypes.AirConditioningEquipment, "indoor.living", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.HallwayLights, "Hallway lights", ApplicationConstants.DeviceTypes.LightsSwitch, "indoor.hallway", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.LivingLights, "Living lights", ApplicationConstants.DeviceTypes.LightsSwitch, "indoor.living", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.HallwayMotionSensor, "Hallway motion", ApplicationConstants.DeviceTypes.MotionSensor, "indoor.hallway", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.LivingMotionSensor, "Living motion", ApplicationConstants.DeviceTypes.MotionSensor, "indoor.living", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.LivingBlinds, "Living blinds", ApplicationConstants.DeviceTypes.BlindsEquipment, "indoor.living", new { on = false }),
            new DomainDevice(ApplicationConstants.DeviceIds.KitchenBlinds, "Kitchen blinds", ApplicationConstants.DeviceTypes.BlindsEquipment, "indoor.kitchen", new { on = false })
        };
        foreach(var device in devices)
        {
            await devicesRepository.AddAsync(device, cancellationToken);
        }
        await devicesRepository.SaveChangesAsync(cancellationToken);
    }
}
