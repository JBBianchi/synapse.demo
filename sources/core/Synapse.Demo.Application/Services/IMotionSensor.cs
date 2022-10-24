namespace Synapse.Demo.Application.Services;

/// <summary>
/// Defines the fundamentals of a service used to interact with motion sensor systems
/// </summary>
public interface IMotionSensor
{

    /// <summary>
    /// Triggers the <see cref="IMotionSensor"/>
    /// </summary>
    /// <param name="sensorId">The id of the sensor that has detected motion</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task TriggerAsync(string sensorId, CancellationToken cancellationToken = default);

}
