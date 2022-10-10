namespace Synapse.Demo.Persistence.UnitTests.Data.Factories;

internal class DomainEventFactory
{
    internal static DeviceCreatedDomainEvent CreateDeviceCreatedDomainEvent()
    {
        return new DeviceCreatedDomainEvent("device-123", "my device", "lamp", @"indoors\\kitchen", new { Hello = "World" }); ;
    }
}
