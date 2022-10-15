namespace Synapse.Demo.WebUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IIntegrationCommand"/>s
/// </summary>
public static class IIntegrationCommandExtensions
{
    /// <summary>
    /// Converts a <see cref="IIntegrationCommand"/> to a <see cref="CloudEventDto"/>
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static CloudEventDto? AsCloudEvent(this IIntegrationCommand command)
    {
        if (!command.GetType().TryGetCustomAttribute(out CloudEventEnvelopeAttribute cloudEventEnvelopeAttribute))
            return null;
        var eventIdentifier = $"{cloudEventEnvelopeAttribute.AggregateType}/{cloudEventEnvelopeAttribute.ActionName}/v1";
        return new(
            Guid.NewGuid().ToString(),
            "web-ui",
            $"{ApplicationConstants.CloudEventsType}/{eventIdentifier}",
            command
       );
    }
}
