namespace Synapse.Demo.Api.WebSocket.IntegrationEventHandlers;

/// <summary>
/// Handles <see cref="Integration.IIntegrationEvent"/> and forward them over the WebSocket
/// </summary>
internal class IntegrationEventHandler
    : INotificationHandler<Integration.IIntegrationEvent>
{
    /// <summary>
    /// Gets the application <see cref="Hub{TClient}"/>
    /// </summary>
    protected IHubContext<DemoApplicationHub, IDemoApplicationClient> HubContext { get; init; }

    /// <summary>
    /// Initializes a new <see cref="IntegrationEventHandler"/>
    /// </summary>
    /// <param name="hubContext">The application <see cref="Hub{TClient}"/></param>
    public IntegrationEventHandler(IHubContext<DemoApplicationHub, IDemoApplicationClient> hubContext)
    {
        if (hubContext == null) throw DomainException.ArgumentNull(nameof(hubContext));
        this.HubContext = hubContext;
    }

    /// <summary>
    /// Handles a <see cref="Integration.IIntegrationEvent"/>
    /// </summary>
    /// <param name="notification">The notification to handle</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandleAsync(Integration.IIntegrationEvent notification, CancellationToken cancellationToken = default)
    {
        await this.HubContext.Clients.All.ReceiveIntegrationEventAsync(notification);
    }
}
