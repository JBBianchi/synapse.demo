namespace Synapse.Demo.Domain;

/// <summary>
/// Represents the base class for all <see cref="IDomainEvent"/>s in the Mozart context
/// </summary>
/// <typeparam name="TAggregate">The type of <see cref="IAggregateRoot"/> that has produced the <see cref="DomainEvent{TAggregate}"/></typeparam>
public abstract class DomainEvent<TAggregate>
    : DomainEvent<TAggregate, string>
    where TAggregate : class, IAggregateRoot<string>
{

    /// <summary>
    /// Initializes a new <see cref="DomainEvent{TAggregate}"/>
    /// </summary>
    protected DomainEvent()
    {}

    /// <summary>
    /// Initializes a new <see cref="DomainEvent{TAggregate}"/>
    /// </summary>
    /// <param name="aggregateId">The id of the <see cref="IAggregateRoot"/> to create the <see cref="DomainEvent{TAggregate}"/> for</param>
    protected DomainEvent(string aggregateId)
        : base(aggregateId)
    {}

}
