using Synapse.Demo.Domain.Models;

namespace Synapse.Demo.Application.UnitTests.Data.Factories;

internal static class DeviceFactory
{
    public static Domain.Models.Device CreateDomainDevice()
    {
        return new Domain.Models.Device("device-123", "my device", "lamp", $"indoors{Domain.Models.Location.LabelSeparator}kitchen", new { Hello = "World" });
    }

    public static Integration.Models.Device CreatePseudoThermometer()
    {
        return new Integration.Models.Device()
        {
            Id = "thermometer",
            Label = "Temperature",
            Type = "sensor.thermometer",
            Location = new Integration.Models.Location() { Label = "indoor" }, 
            State = new { temperature = 17, desired = 19 }
        };
    }
}
