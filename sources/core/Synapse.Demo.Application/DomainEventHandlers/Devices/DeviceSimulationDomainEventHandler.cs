namespace Synapse.Demo.Application.DomainEventHandlers.Devices
{

    /// <summary>
    /// Handles <see cref="IDomainEvent"/>s related to <see cref="Domain.Models.Device"/> simulation
    /// </summary>
    internal class DeviceSimulationDomainEventHandler
        : DomainEventHandlerBase<Domain.Models.Device, Device, string>,
        INotificationHandler<DeviceStateChangedDomainEvent>
    {

        /// <summary>
        /// Initializes a new <see cref="DeviceSimulationDomainEventHandler"/>
        /// </summary>
        /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
        /// <param name="options">The applicaiton <see cref="IOptions{TOptions}"/></param>
        /// <param name="mapper">The service used to map objects</param>
        /// <param name="mediator">The service used to mediate calls</param>
        /// <param name="cloudEventBus">The service used to publish and subscribe to <see cref="CloudEvent"/>s</param>
        /// <param name="cloudEventStream">The <see cref="Subject{T}"/> used to observe consumed <see cref="CloudEvent"/>s</param>
        /// <param name="airConditioner">The service used to manage Air Conditioning</param>
        /// <param name="heater">The service used to manage the heating systems</param>
        /// <param name="motionSensor">The service used to manage motion sensor systems</param>
        public DeviceSimulationDomainEventHandler(ILoggerFactory loggerFactory, IOptions<DemoApplicationOptions> options, IMapper mapper, IMediator mediator, 
            ICloudEventBus cloudEventBus, ISubject<CloudEvent> cloudEventStream, IRepository<Domain.Models.Device, string> writeModels, IRepository<Device, string> readModels,
            IAirConditioner airConditioner, IHeater heater, IMotionSensor motionSensor) 
            : base(loggerFactory, options, mapper, mediator, cloudEventBus, cloudEventStream, writeModels, readModels)
        {
            this.AirConditioner = airConditioner;
            this.Heater = heater;
            this.MotionSensor = motionSensor;
        }

        /// <summary>
        /// Gets the service used to manage air conditioning systems
        /// </summary>
        protected IAirConditioner AirConditioner { get; }

        /// <summary>
        /// Gets the service used to manage heating systems
        /// </summary>
        protected IHeater Heater { get; }

        /// <summary>
        /// Gets the service used to manage motion sensor systems
        /// </summary>
        protected IMotionSensor MotionSensor { get; }

        async Task INotificationHandler<DeviceStateChangedDomainEvent>.HandleAsync(DeviceStateChangedDomainEvent e, CancellationToken cancellationToken)
        {
            bool turnedOn;
            switch (e.AggregateId)
            {
                case ApplicationConstants.DeviceIds.AirConditioning:
                    turnedOn = ((bool?)((dynamic?)e.State)?.on) == true;
                    if (turnedOn) await this.AirConditioner.TurnOnAsync(cancellationToken);
                    else await this.AirConditioner.TurnOffAsync(cancellationToken);
                    break;
                case ApplicationConstants.DeviceIds.Heater:
                    turnedOn = ((bool?)((dynamic?)e.State)?.on) == true;
                    if (turnedOn) await this.Heater.TurnOnAsync(cancellationToken);
                    else await this.Heater.TurnOffAsync(cancellationToken);
                    break;
                default:
                    if (e.AggregateId.StartsWith("motion-sensor")) await this.MotionSensor.TriggerAsync(e.AggregateId, cancellationToken);
                    break;
            }
        }

    }

}
