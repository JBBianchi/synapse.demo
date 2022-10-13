using Neuroglia;
using Synapse.Demo.Common;
using Synapse.Demo.Integration.Attributes;
using Synapse.Demo.Integration.Commands;
using Synapse.Demo.Integration.Models;

namespace Synapse.Demo.WebUI.Extensions;

public static class IIntegrationCommandExtensions
{
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
