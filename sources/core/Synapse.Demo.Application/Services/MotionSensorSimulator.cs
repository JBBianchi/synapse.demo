using Synapse.Demo.Application.Commands.Devices;
using System.Collections.Concurrent;

namespace Synapse.Demo.Application.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IMotionSensor"/> interface
/// </summary>
public class MotionSensorSimulator
    : BackgroundService,
    IMotionSensor
{

    /// <summary>
    /// Initializes a new <see cref="MotionSensorSimulator"/>
    /// </summary>
    /// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
    /// <param name="logger">The service used to perform logging</param>
    public MotionSensorSimulator(IServiceProvider serviceProvider, ILogger<MotionSensorSimulator> logger)
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

    /// <summary>
    /// Gets an <see cref="ConcurrentDictionary{TKey, TValue}"/> containing the states of the motion sensors mapped by id
    /// </summary>
    protected ConcurrentDictionary<string, bool> SensorStates { get; } = new();

    /// <summary>
    /// Gets the <see cref="MotionSensorSimulator"/>'s <see cref="System.Threading.CancellationTokenSource"/>
    /// </summary>
    protected CancellationTokenSource CancellationTokenSource { get; private set; } = null!;

    /// <summary>
    /// Gets the <see cref="MotionSensorSimulator"/>'s detection <see cref="System.Threading.CancellationTokenSource"/>
    /// </summary>
    protected CancellationTokenSource? DetectionCancellationTokenSource { get; private set; }

    /// <inheritdoc/>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task TriggerAsync(string sensorId, CancellationToken cancellationToken = default)
    {
        if(!this.SensorStates.TryGetValue(sensorId, out var isTriggered)) this.SensorStates.TryAdd(sensorId, true);
        if (!isTriggered) _ = this.SenseMotionAsync(sensorId);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Senses motion
    /// </summary>
    /// <param name="sensorId">The id of the sensor that has detected motion</param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    protected virtual async Task SenseMotionAsync(string sensorId)
    {
        try
        {
            this.DetectionCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.CancellationTokenSource.Token);
            this.SensorStates.AddOrUpdate(sensorId, true, (_, _) => true);
            using var scope = this.ServiceProvider.CreateScope();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await Task.Delay(2000);
            await mediator.ExecuteAsync(new UpdateDeviceStateCommand(sensorId, new { on = false }));
        }
        catch (TaskCanceledException) { }
        catch (Exception ex)
        {
            this.Logger.LogError("An error occured while sensing motion: {ex}", ex);
        }
        finally
        {
            this.SensorStates.AddOrUpdate(sensorId, false, (_, _) => false);
        }
    }

}