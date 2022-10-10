

namespace Synapse.Demo.Persistence.UnitTests.Cases.Write;

public class InMemoryEventStoreTests
{
    [Fact]
    public async void AppendToStream_Should_Work()
    {
        var eventStore = EventStoreFactory.Create();
        var streamId = "test-stream";
        var domainEvent = DomainEventFactory.CreateDeviceCreatedDomainEvent();
        var events = new EventMetadata[]
        {
            new ("FakeType", domainEvent)
        };

        await eventStore.AppendToStreamAsync(streamId, events);
        var sourcedEvents = await eventStore.ReadAllEventsForwardAsync(streamId);

        sourcedEvents.Should().NotBeNull();
        sourcedEvents.Should().HaveCount(events.Length);
        sourcedEvents.First().Data.As<DeviceCreatedDomainEvent>().Should().BeEquivalentTo(events.First().Data);
    }

    [Fact]
    public async void AppendToStream_Wrong_Version_Should_Throw()
    {
        var eventStore = EventStoreFactory.Create();
        var streamId = "test-stream";
        var domainEvent = DomainEventFactory.CreateDeviceCreatedDomainEvent();
        var events = new EventMetadata[]
        {
            new ("FakeType", domainEvent)
        };

        await eventStore.AppendToStreamAsync(streamId, events);
        var act = () => eventStore.AppendToStreamAsync(streamId, events);

        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async void AppendToStream_Expected_Version_Should_Work()
    {
        var eventStore = EventStoreFactory.Create();
        var streamId = "test-stream";
        var domainEvent = DomainEventFactory.CreateDeviceCreatedDomainEvent();
        var events = new EventMetadata[]
        {
            new ("FakeType", domainEvent),
            new ("FakeType", domainEvent)
        };

        await eventStore.AppendToStreamAsync(streamId, events);
        await eventStore.AppendToStreamAsync(streamId, events, 1);
        var sourcedEvents = await eventStore.ReadAllEventsForwardAsync(streamId);

        sourcedEvents.Should().NotBeNull();
        sourcedEvents.Should().HaveCount(events.Length * 2);
        sourcedEvents.Last().Sequence.Should().Be(3);
        sourcedEvents.Last().Data.As<DeviceCreatedDomainEvent>().Should().BeEquivalentTo(events.Last().Data);
    }
}
