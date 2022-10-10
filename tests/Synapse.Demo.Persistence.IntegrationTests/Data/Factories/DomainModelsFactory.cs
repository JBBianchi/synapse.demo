using Synapse.Demo.Domain.Models;

namespace Synapse.Demo.Persistence.IntegrationTests.Data.Factories;

internal class DomainModelsFactory
{
    internal static Device CreateDevice()
    {
        return new Device(
            id: "device-123",
            label: "my device",
            type: "lamp",
            location: @"indoors\\kitchen",
            state: new { Hello = "World" }
        );
    }
}
