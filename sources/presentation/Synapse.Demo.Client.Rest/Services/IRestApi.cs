using System.Threading.Tasks;

namespace Synapse.Demo.Client.Rest.Services;

/// <summary>
/// Represents the service used to interact with the HTTP REST API
/// </summary>
public interface IRestApiClient
{
    /// <summary>
    /// Creates a new <see cref="Device"/>
    /// </summary>
    /// <param name="command">The <see cref="CreateDeviceCommand"/> used to create the device</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The created <see cref="Device"/></returns>
    Task<Device> CreateDevice(CreateDeviceCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Queries the <see cref="Device"/>s
    /// </summary>
    /// <param name="query">The potential OData query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A list of <see cref="Device"/>s</returns>
    Task<IEnumerable<Device>> GetDevices(string? query = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the <see cref="Device"/> with the specified id.
    /// </summary>
    /// <param name="id">The id of the <see cref="Device"/> to find</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The targeted <see cref="Device"/></returns>
    Task<Device> GetDeviceById(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a <see cref="Device"/> state
    /// </summary>
    /// <param name="command">The <see cref="UpdateDeviceStateCommand"/> used to update the device state</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The updated <see cref="Device"/></returns>
    Task<Device> UpdateDeviceState(UpdateDeviceStateCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Patch a <see cref="Device"/> state
    /// </summary>
    /// <param name="command">The <see cref="PatchDeviceStateCommand"/> used to patch the device state</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The updated <see cref="Device"/></returns>
    Task<Device> PatchDeviceState(PatchDeviceStateCommand command, CancellationToken cancellationToken = default);
}