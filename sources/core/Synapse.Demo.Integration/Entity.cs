namespace Synapse.Demo.Integration;

/// <summary>
/// Represents the base class of all the application's <see cref="IEntity"/> implementations
/// </summary>
public class Entity
    : Entity<string>
{

    /// <summary>
    /// Initializes a new <see cref="Entity"/>
    /// </summary>
    protected Entity()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Entity"/>
    /// </summary>
    /// <param name="id">The <see cref="Entity"/>'s unique identifier</param>
    public Entity(string id) : base(id)
    {
    }
}
