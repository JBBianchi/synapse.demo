namespace Synapse.Demo.Api.WebSocket.Hubs;

/// <summary>
/// Defines the WebSocket API 
/// </summary>
public interface IDemoApplicationClient
{
    /// <summary>
    /// Receives an <see cref="CloudEventDto"/>
    /// </summary>
    /// <param name="e">The received <see cref="CloudEventDto"/></param>
    Task ReceiveIntegrationEventAsync(CloudEventDto e);
}
