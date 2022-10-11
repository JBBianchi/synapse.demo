namespace Synapse.Demo.Api.WebSocket.Hubs;

/// <summary>
/// Defines the WebSocket API 
/// </summary>
public interface IDemoApplicationClient
{
    /// <summary>
    /// Receives an <see cref="Integration.IIntegrationEvent"/>
    /// </summary>
    /// <param name="e">The received <see cref="Integration.IIntegrationEvent"/></param>
    Task ReceiveIntegrationEventAsync(Integration.IIntegrationEvent e);
}
