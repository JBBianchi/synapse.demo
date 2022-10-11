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
    protected DemoApplicationHub Hub { get; init; }

    /// <summary>
    /// Initializes a new <see cref="IntegrationEventHandler"/>
    /// </summary>
    /// <param name="hub">The application <see cref="Hub{TClient}"/></param>
    public IntegrationEventHandler(DemoApplicationHub hub)
    {
        if (hub == null) throw DomainException.ArgumentNull(nameof(hub));
        this.Hub = hub;
    }

    /// <summary>
    /// Handles a <see cref="Integration.IIntegrationEvent"/>
    /// </summary>
    /// <param name="notification">The notification to handle</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandleAsync(Integration.IIntegrationEvent notification, CancellationToken cancellationToken = default)
    {
        await this.Hub.Clients.All.ReceiveIntegrationEventAsync(notification);
    }
}
