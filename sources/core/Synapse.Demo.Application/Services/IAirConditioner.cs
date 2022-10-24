namespace Synapse.Demo.Application.Services;

/// <summary>
/// Defines the fundamentals of a service used to interact with AC systems
/// </summary>
public interface IAirConditioner
{

    /// <summary>
    /// Gets a boolean indicating whether or not the <see cref="IAirConditioner"/> is powered on
    /// </summary>
    bool IsPoweredOn { get; }

    /// <summary>
    /// Powers the AC on
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task TurnOnAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Powers the AC off
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task TurnOffAsync(CancellationToken cancellationToken = default);

}
