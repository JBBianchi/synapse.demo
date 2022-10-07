namespace Synapse.Demo.Integration;

public interface IIntegrationEvent
    : Neuroglia.IIntegrationEvent
{
    /// <summary>
    /// Gets the id of the aggregate that has produced the event
    /// </summary>
    string AggregateId { get; set; }

    /// <summary>
    /// Gets the date and time at which the event has been created
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }
}
