namespace Synapse.Demo.Application.Services;

/// <summary>
/// Defines the fundamentals of a service used to interact with heating systems
/// </summary>
public interface IHeater
{

    /// <summary>
    /// Gets a boolean indicating whether or not the <see cref="IHeater"/> is powered on
    /// </summary>
    bool IsPoweredOn { get; }

    /// <summary>
    /// Powers the heater on
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task TurnOnAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Powers the heater off
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task TurnOffAsync(CancellationToken cancellationToken = default);

}
