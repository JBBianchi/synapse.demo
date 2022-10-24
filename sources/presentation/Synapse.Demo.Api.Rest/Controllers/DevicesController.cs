namespace Synapse.Demo.Api.Rest.Controllers;

/// <summary>
/// Represents the <see cref="RestApiController"/> used to managed <see cref="Device"/>s
/// </summary>
[Route("api/v1/devices")]
public class DevicesController
    : RestApiController
{
    /// <summary>
    /// Initializes a new <see cref="DevicesController"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    public DevicesController(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper)
        : base(loggerFactory, mediator, mapper)
    {
    }

    /// <summary>
    /// Queries <see cref="Device"/>s
    /// <para>This endpoint supports ODATA.</para>
    /// </summary>
    /// <param name="queryOptions">The options of the ODATA query to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/></returns>
    [HttpGet, EnableQuery]
    [ProducesResponseType(typeof(IEnumerable<Device>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDevices(ODataQueryOptions<Device> queryOptions, CancellationToken cancellationToken)
    {
        return this.Process(await this.Mediator.ExecuteAsync(new GenericFilterQuery<Device>(queryOptions), cancellationToken));
    }

    /// <summary>
    /// Gets the <see cref="Device"/> with the specified id.
    /// </summary>
    /// <param name="id">The id of the <see cref="Device"/> to find</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IActionResult"/></returns>
    [HttpGet("{id}"), EnableQuery]
    [ProducesResponseType(typeof(Device), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetDeviceById(string id, CancellationToken cancellationToken)
    {
        return this.Process(await this.Mediator.ExecuteAsync(new GenericFindByIdQuery<Device, string>(id), cancellationToken));
    }

    /// <summary>
    /// Creates a new <see cref="Device"/>
    /// </summary>
    /// <param name="command">An object that represents the command to execute</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Device), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateDevice([FromBody] CreateDeviceCommand command, CancellationToken cancellationToken)
    {
        return this.Process(await this.Mediator.ExecuteAsync(this.Mapper.Map<Application.Commands.Devices.CreateDeviceCommand>(command), cancellationToken));
    }

    /// <summary>
    /// Updates a <see cref="Device"/>'s state
    /// </summary>
    /// <param name="command">An object that represents the command to execute</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(Device), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateDeviceState([FromBody] UpdateDeviceStateCommand command, CancellationToken cancellationToken)
    {
        return this.Process(await this.Mediator.ExecuteAsync(this.Mapper.Map<Application.Commands.Devices.UpdateDeviceStateCommand>(command), cancellationToken));
    }

    /// <summary>
    /// Patches a <see cref="Device"/>'s state
    /// </summary>
    /// <param name="command">An object that represents the command to execute</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns></returns>
    [HttpPatch]
    [ProducesResponseType(typeof(Device), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PatchDeviceState([FromBody] PatchDeviceStateCommand command, CancellationToken cancellationToken)
    {
        return this.Process(await this.Mediator.ExecuteAsync(this.Mapper.Map<Application.Commands.Devices.PatchDeviceStateCommand>(command), cancellationToken));
    }
}
