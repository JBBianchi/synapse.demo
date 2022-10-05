namespace Synapse.Demo.Domain;


/// <summary>
/// Represents the base class of all the application's <see cref="IAggregateRoot"/> implementations
/// </summary>
public abstract class AggregateRoot
    : AggregateRoot<string>
{

    /// <summary>
    /// Initializes a new <see cref="AggregateRoot"/>
    /// </summary>
    /// <param name="id">The <see cref="AggregateRoot"/>'s unique identifier</param>
    protected AggregateRoot(string id)
        : base(id)
    {

    }

}