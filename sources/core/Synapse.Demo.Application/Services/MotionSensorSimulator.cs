using Neuroglia.Mediation;
using Synapse.Demo.Application.Commands.Devices;
using static System.Formats.Asn1.AsnWriter;

namespace Synapse.Demo.Application.Services;

/// <summary>
/// The service used to emulate a motion sensor (resets to its default state automatically)
/// </summary>
internal class MotionSensorSimulator
    : INotificationHandler<DeviceStateChangedDomainEvent>
{
    /// <summary>
    /// Gets the <see cref="ILogger/>
    /// </summary>
    protected ILogger Logger { get; init; }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the (read) <see cref="IRepository"/> used to manage the <see cref="Device"/>s
    /// </summary>
    protected IRepository<Device, string> Devices { get; init; }

    /// <summary>
    /// Gets the service used to map objects
    /// </summary>
    protected IMapper Mapper { get; init; }

    public static CancellationTokenSource? CancellationTokenSource = null;
    private string _motionSensorIdPrefix = "motion-sensor";

    /// <summary>
    /// Initializes a <see cref="AcSimulator"/>
    /// </summary>
    /// <param name="logger">The service used to log</param>
    /// <param name="devices">The <see cref="IRepository"/> used to manage the <see cref="Device"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    public MotionSensorSimulator(ILogger<AcSimulator> logger, IServiceProvider serviceProvider, IRepository<Device, string> devices, IMapper mapper)
    {
        if (logger == null) throw DomainException.ArgumentNull(nameof(logger));
        if (devices == null) throw DomainException.ArgumentNull(nameof(devices));
        if (mapper == null) throw DomainException.ArgumentNull(nameof(mapper));
        this.Logger = logger;
        this.ServiceProvider = serviceProvider;
        this.Devices = devices;
        this.Mapper = mapper;
    }

    /// <summary>
    /// Handles a <see cref="DeviceStateChangedDomainEvent"/> notification
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandleAsync(DeviceStateChangedDomainEvent e, CancellationToken cancellationToken = default)
    {
        if (!e.AggregateId.StartsWith(this._motionSensorIdPrefix)) return;
        var turnedOn = ((dynamic?)e.State)?.on != null && (bool)((dynamic?)e.State)?.on;
        if (turnedOn)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = this.ServiceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await Task.Delay(2000);
                    await mediator.ExecuteAsync(new UpdateDeviceStateCommand(e.AggregateId, new { on = false }));
                }
                catch (Exception ex)
                {
                    this.Logger.LogError("The motion sensor experienced an exception '{ex}'", ex);
                }
            }, cancellationToken);
        }
    }
}
