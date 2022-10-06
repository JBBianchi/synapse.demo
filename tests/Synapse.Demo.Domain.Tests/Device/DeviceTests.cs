namespace Synapse.Demo.Domain.UnitTests
{
    public class DeviceTests
    {
        [Fact]
        public void ConstructorShouldWork()
        {
            var id = "device-123";
            var label = "my device";
            var type = "lamp";
            var location = @"indoors\\kitchen";

            var device = new Device(id, label, type, location, null);

            device.Should().NotBeNull();
            device.Id.Should().Be(id);
            device.Label.Should().Be(label);
            device.Location.Should().Be(location);
            device.Type.Should().Be(type);
            device.State.Should().BeNull();
        }

        [Fact]
        public void ConstructorWithStateShouldWork()
        {
            var id = "device-123";
            var label = "my device";
            var location = @"indoors\\kitchen";
            var type = "lamp";
            var state = new { Hello = "World" };

            var device = new Device(id, label, type, location, state);

            device.Should().NotBeNull();
            device.Id.Should().Be(id);
            device.Label.Should().Be(label);
            device.Location.Should().Be(location);
            device.Type.Should().Be(type);
            device.State.Should().Be(state);
        }

        [Fact]
        public void NullLocationShouldThrow()
        {
            var id = "device-123";
            var label = "my device";
            var type = "lamp";

            var task = () => new Device(id, label, type, null!, null);

            task.Should().Throw<NullDeviceLocationDomainException>();
        }

        [Fact]
        public void EmptyLocationShouldThrow()
        {
            var id = "device-123";
            var label = "my device";
            var type = "lamp";

            var task = () => new Device(id, label, type, " ", null);

            task.Should().Throw<NullDeviceLocationDomainException>();
        }

        [Fact]
        public void NullTypeShouldThrow()
        {
            var id = "device-123";
            var label = "my device";
            var location = @"indoors\\kitchen";

            var task = () => new Device(id, label, null!, location, null);

            task.Should().Throw<NullDeviceTypeDomainException>();
        }

        [Fact]
        public void EmptyTypeShouldThrow()
        {
            var id = "device-123";
            var label = "my device";
            var location = @"indoors\\kitchen";

            var task = () => new Device(id, label, " ", location, null);

            task.Should().Throw<NullDeviceTypeDomainException>();
        }

        [Fact]
        public void NullLabelShouldThrow()
        {
            var id = "device-123";
            var type = "lamp";
            var location = @"indoors\\kitchen";

            var task = () => new Device(id, null!, type, location, null);

            task.Should().Throw<NullDeviceLabelDomainException>();
        }

        [Fact]
        public void EmptyLabelShouldThrow()
        {
            var id = "device-123";
            var type = "lamp";
            var location = @"indoors\\kitchen";

            var task = () => new Device(id, " ", type, location, null);

            task.Should().Throw<NullDeviceLabelDomainException>();
        }

        [Fact]
        public void NullIdShouldThrow()
        {
            var label = "my device";
            var type = "lamp";
            var location = @"indoors\\kitchen";

            var task = () => new Device(null!, label, type, location, null);

            task.Should().Throw<NullDeviceIdDomainException>();
        }

        [Fact]
        public void EmptyIdShouldThrow()
        {
            var label = "my device";
            var type = "lamp";
            var location = @"indoors\\kitchen";

            var task = () => new Device(" ", label ,type, location, null);

            task.Should().Throw<NullDeviceIdDomainException>();
        }
    }
}
