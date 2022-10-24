using Synapse.Demo.Application.Commands.Devices;

namespace Synapse.Demo.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IAirConditioner"/> interface
/// </summary>
public class AirConditionerSimulator
    : BackgroundService,
    IAirConditioner
{
   
    /// <summary>
    /// Initializes a new <see cref="AirConditionerSimulator"/>
    /// </summary>
    /// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
    /// <param name="logger">The service used to perform logging</param>
    public AirConditionerSimulator(IServiceProvider serviceProvider, ILogger<AirConditionerSimulator> logger)
    {
        this.ServiceProvider = serviceProvider;
        this.Logger = logger;
    }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; }

    /// <inheritdoc/>
    public bool IsPoweredOn { get; private set; }

    /// <summary>
    /// Gets the <see cref="AirConditionerSimulator"/>'s <see cref="System.Threading.CancellationTokenSource"/>
    /// </summary>
    protected CancellationTokenSource CancellationTokenSource { get; private set; } = null!;

    /// <summary>
    /// Gets the <see cref="AirConditionerSimulator"/>'s <see cref="System.Threading.CancellationTokenSource"/>
    /// </summary>
    protected CancellationTokenSource? CoolingCancellationTokenSource { get; private set; }

    /// <inheritdoc/>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task TurnOnAsync(CancellationToken cancellationToken = default)
    {
        if (!this.IsPoweredOn) _ = this.CoolAsync();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual async Task TurnOffAsync(CancellationToken cancellationToken = default)
    {
        if (!this.IsPoweredOn || this.CoolingCancellationTokenSource == null) return;
        this.CoolingCancellationTokenSource.Cancel();
        this.CoolingCancellationTokenSource = null;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Cools the air
    /// </summary>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    protected virtual async Task CoolAsync()
    {
        try
        {
            this.CoolingCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.CancellationTokenSource.Token);
            this.IsPoweredOn = true;
            using var scope = this.ServiceProvider.CreateScope();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var devices = scope.ServiceProvider.GetRequiredService<IRepository<Device, string>>();
            var device = await devices.FindAsync(ApplicationConstants.DeviceIds.Thermometer);
            this.Logger.LogInformation("AC is now on");
            Thermometer thermometer = null!;
            if (device != null) thermometer = mapper.Map<Thermometer>(device);
            if (device == null || !thermometer.Temperature.HasValue)
            {
                await mediator.ExecuteAsync(new UpdateDeviceStateCommand(ApplicationConstants.DeviceIds.AirConditioning, new { on = false }));
                return;
            }
            var temperature = thermometer.Temperature;
            var desiredTemperature = thermometer.DesiredTemperature;
            while (!this.CancellationTokenSource.IsCancellationRequested)
            {
                temperature--;
                var state = new
                {
                    temperature = temperature,
                    desired = desiredTemperature
                };
                await mediator.ExecuteAsync(new UpdateDeviceStateCommand(thermometer.Id, state));
                await Task.Delay(2000);
            }
        }
        catch (TaskCanceledException) { }
        catch (Exception ex)
        {
            this.Logger.LogError("An error occured while cooling the air: {ex}", ex);
        }
        finally
        {
            this.IsPoweredOn = false;
            this.Logger.LogInformation("AC is now off");
        }
    }

}
