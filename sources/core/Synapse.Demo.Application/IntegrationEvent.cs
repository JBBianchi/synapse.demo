namespace Synapse.Demo.Application;

/// <summary>
/// Represents the base class for all <see cref="IIntegrationEvent"/>s
/// </summary>
public abstract class IntegrationEvent
    : IIntegrationEvent
{

    /// <summary>
    /// Gets the id of the aggregate that has produced the event
    /// </summary>
    public virtual string AggregateId { get; set; }

    /// <summary>
    /// Gets the date and time at which the event has been created
    /// </summary>
    public virtual DateTimeOffset CreatedAt { get; set; }

}