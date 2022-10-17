using Synapse.Demo.Application.Commands.Devices;
using static System.Formats.Asn1.AsnWriter;

namespace Synapse.Demo.Application.Services;

/// <summary>
/// The service used to emulate the effects of a heater
/// </summary>
internal class HeaterSimulator
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
    private string _heaterId = "heater";
    private string _thermometerId = "thermometer";

    /// <summary>
    /// Initializes a <see cref="HeaterSimulator"/>
    /// </summary>
    /// <param name="logger">The service used to log</param>
    /// <param name="devices">The <see cref="IRepository"/> used to manage the <see cref="Device"/>s</param>
    /// <param name="mediator">The service used to mediate calls</param>
    /// <param name="mapper">The service used to map objects</param>
    public HeaterSimulator(ILogger<HeaterSimulator> logger, IServiceProvider serviceProvider, IRepository<Device, string> devices, IMapper mapper)
    {
        if (logger == null) throw DomainException.ArgumentNull(nameof(logger));
        if (serviceProvider == null) throw DomainException.ArgumentNull(nameof(serviceProvider));
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
        if (e.AggregateId != this._heaterId) return;
        var turnedOn = ((dynamic?)e.State)?.on != null && (bool)((dynamic?)e.State)?.on;
        if (!turnedOn) {
            if (HeaterSimulator.CancellationTokenSource != null)
            {
                HeaterSimulator.CancellationTokenSource.Cancel();
                HeaterSimulator.CancellationTokenSource.Dispose();
                HeaterSimulator.CancellationTokenSource = null;
            }
        }
        else if (HeaterSimulator.CancellationTokenSource == null)
        {
            HeaterSimulator.CancellationTokenSource = new CancellationTokenSource();
            var token = HeaterSimulator.CancellationTokenSource.Token;
            var _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = this.ServiceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var device = await this.Devices.FindAsync(this._thermometerId);
                    Thermometer thermometer = null!;
                    if (device != null)
                    {
                        thermometer = this.Mapper.Map<Thermometer>(device);
                    }
                    if (
                        thermometer == null
                    || !thermometer.Temperature.HasValue
                    || !thermometer.DesiredTemperature.HasValue
                    || thermometer.Temperature == thermometer.DesiredTemperature
                    )
                    {
                        await mediator.ExecuteAsync(new UpdateDeviceStateCommand(this._heaterId, new { on = false }));
                        return;
                    }
                    var temperature = thermometer.Temperature;
                    var desiredTemperature = thermometer.DesiredTemperature;
                    while (temperature < desiredTemperature)
                    {
                        temperature++;
                        await mediator.ExecuteAsync(new UpdateDeviceStateCommand(thermometer.Id, new
                        {
                            temperature = temperature,
                            desired = desiredTemperature
                        }));
                        await Task.Delay(1000);
                    }
                    await mediator.ExecuteAsync(new UpdateDeviceStateCommand(this._heaterId, new { on = false }));
                }
                catch(Exception ex)
                {
                    this.Logger.LogError("The heater experienced an exception '{ex}'", ex);
                }
            }, cancellationToken);
        }
    }
}
