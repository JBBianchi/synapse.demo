using Synapse.Demo.Domain.Models;
using Synapse.Demo.Integration.Models;

namespace Synapse.Demo.Application.UnitTests.Cases.Mapping;

/// <summary>
/// Holds the tests for a <see cref="Device"/>
/// </summary>
public class MappingProfileTests
{
    /// <summary>
    /// Mapping a domain model to an integration model should work
    /// </summary>
    [Fact]
    public void Domain_Model_To_Integration_Should_Work()
    {
        var mapper = MapperFactory.Create();
        var domainModel = DeviceFactory.CreateDomainDevice();

        var integrationModel = mapper.Map<Integration.Models.Device>(domainModel);

        integrationModel.Should().NotBeNull();
        integrationModel.Id.Should().Be(domainModel.Id);
        integrationModel.Label.Should().Be(domainModel.Label);
        integrationModel.Type.Should().Be(domainModel.Type);
        integrationModel.Location.Should().NotBeNull();
        integrationModel.Location.Label.Should().Be("kitchen");
        integrationModel.Location.Parent.Should().NotBeNull();
        integrationModel.Location.Parent!.Label.Should().Be("indoors");
        integrationModel.State.Should().Be(domainModel.State);
    }

    /// <summary>
    /// Mapping a domain event to an integration event should work
    /// </summary>
    [Fact]
    public void Domain_Events_To_Integration_Should_Work()
    {
        var mapper = MapperFactory.Create();
        var domainEvent = DeviceFactory.CreateDomainDevice();

        var integrationEvent = mapper.Map<DeviceCreatedIntegrationEvent>(domainEvent);

        integrationEvent.Should().NotBeNull();
        integrationEvent.Id.Should().Be(domainEvent.Id);
        integrationEvent.Label.Should().Be(domainEvent.Label);
        integrationEvent.Type.Should().Be(domainEvent.Type);
        integrationEvent.Location.Label.Should().Be("kitchen");
        integrationEvent.Location.Parent.Should().NotBeNull();
        integrationEvent.Location.Parent!.Label.Should().Be("indoors");
        integrationEvent.State.Should().Be(domainEvent.State);
    }

    /// <summary>
    /// Mapping a integration device to a specialized device "thermometer"
    /// </summary>
    [Fact]
    public void Integration_Device_To_Specialized_Device_Should_Work()
    {
        var mapper = MapperFactory.Create();
        var device = DeviceFactory.CreatePseudoThermometer();

        var thermometer = mapper.Map<Thermometer>(device);

        thermometer.Should().NotBeNull();
        thermometer.Id.Should().Be(device.Id);
        thermometer.Label.Should().Be(device.Label);
        thermometer.Type.Should().Be(device.Type);
        thermometer.Location.Label.Should().Be(device.Location.Label);
        thermometer.Temperature.Should().Be(((dynamic?)device.State)!.temperature);
        thermometer.DesiredTemperature.Should().Be(((dynamic?)device.State)!.desired);
    }
}
