namespace Synapse.Demo.Domain.UnitTests;

/// <summary>
/// Holds the tests for <see cref="Device"/>
/// </summary>
public class DeviceTests
{
    /// <summary>
    /// Holds the data used to instanciate a <see cref="Device"/>
    /// </summary>
    public readonly static object MockDevice = new {
        Id = "device-123",
        Label = "my device",
        Type = "lamp",
        Location = @"indoors\\kitchen",
        State = new { Hello = "World" }
    };

    /// <summary>
    /// Valid constructor arguments, without state, should work
    /// </summary>
    [Fact]
    public void Constructor_Without_State_Should_Work()
    {
        var mockDevice = (dynamic)MockDevice;

        var device = new Device(mockDevice.Id, mockDevice.Label, mockDevice.Type, mockDevice.Location, null);

        device.Should().NotBeNull();
        device.Id.Should().Be(mockDevice.Id);
        device.Label.Should().Be(mockDevice.Label);
        device.Type.Should().Be(mockDevice.Type);
        device.Location.Should().Be(mockDevice.Location);
        device.State.Should().BeNull();
    }

    /// <summary>
    /// Valid constructor arguments, with a state, should work
    /// </summary>
    [Fact]
    public void Constructor_With_State_Should_Work()
    {
        var mockDevice = (dynamic)MockDevice;

        var device = new Device(mockDevice.Id, mockDevice.Label, mockDevice.Type, mockDevice.Location, mockDevice.State);

        device.Should().NotBeNull();
        device.Id.Should().Be(mockDevice.Id);
        device.Label.Should().Be(mockDevice.Label);
        device.Type.Should().Be(mockDevice.Type);
        device.Location.Should().Be(mockDevice.Location);
        device.State.Should().Be(mockDevice.State);
    }

    /// <summary>
    /// Invalid constructor arguments, without location, should throw
    /// </summary>
    [Fact]
    public void Null_Location_Should_Throw()
    {
        var mockDevice = (dynamic)MockDevice;

        var task = () => new Device(mockDevice.Id, mockDevice.Label, mockDevice.Type, null!, null);

        task.Should().Throw<NullDeviceLocationDomainException>();
    }

    /// <summary>
    /// Invalid constructor arguments, with an empty location, should throw
    /// </summary>
    [Fact]
    public void Empty_Location_Should_Throw()
    {
        var mockDevice = (dynamic)MockDevice;

        var task = () => new Device(mockDevice.Id, mockDevice.Label, mockDevice.Type, " ", null);

        task.Should().Throw<NullDeviceLocationDomainException>();
    }

    /// <summary>
    /// Invalid constructor arguments, without type, should throw
    /// </summary>
    [Fact]
    public void Null_Type_Should_Throw()
    {
        var mockDevice = (dynamic)MockDevice;

        var task = () => new Device(mockDevice.Id, mockDevice.Label, null!, mockDevice.Location, null);

        task.Should().Throw<NullDeviceTypeDomainException>();
    }

    /// <summary>
    /// Invalid constructor arguments, with an empty type, should throw
    /// </summary>
    [Fact]
    public void Empty_Type_Should_Throw()
    {
        var mockDevice = (dynamic)MockDevice;

        var task = () => new Device(mockDevice.Id, mockDevice.Label, " ", mockDevice.Location, null);

        task.Should().Throw<NullDeviceTypeDomainException>();
    }

    /// <summary>
    /// Invalid constructor arguments, without label, should throw
    /// </summary>
    [Fact]
    public void Null_Label_Should_Throw()
    {
        var mockDevice = (dynamic)MockDevice;

        var task = () => new Device(mockDevice.Id, null!, mockDevice.Type, mockDevice.Location, null);

        task.Should().Throw<NullDeviceLabelDomainException>();
    }

    /// <summary>
    /// Invalid constructor arguments, with an empty label, should throw
    /// </summary>
    [Fact]
    public void Empty_Label_Should_Throw()
    {
        var mockDevice = (dynamic)MockDevice;

        var task = () => new Device(mockDevice.Id, " ", mockDevice.Type, mockDevice.Location, null);

        task.Should().Throw<NullDeviceLabelDomainException>();
    }

    /// <summary>
    /// Invalid constructor arguments, without id, should throw
    /// </summary>
    [Fact]
    public void Null_Id_Should_Throw()
    {
        var mockDevice = (dynamic)MockDevice;

        var task = () => new Device(null!, mockDevice.Label, mockDevice.Type, mockDevice.Location, null);

        task.Should().Throw<NullDeviceIdDomainException>();
    }

    /// <summary>
    /// Invalid constructor arguments, with an empty id, should throw
    /// </summary>
    [Fact]
    public void Empty_Id_Should_Throw()
    {
        var mockDevice = (dynamic)MockDevice;

        var task = () => new Device(" ", mockDevice.Label, mockDevice.Type, mockDevice.Location, null);

        task.Should().Throw<NullDeviceIdDomainException>();
    }
}
