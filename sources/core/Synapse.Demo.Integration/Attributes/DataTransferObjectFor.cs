namespace Synapse.Demo.Integration.Attributes;

/// <summary>
/// Represents an <see cref="Attribute"/> used to specify the entity for a data transfer object
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DataTransferObjectForAttribute
    : Attribute
{
    /// <summary>
    /// Initializes a new <see cref="DataTransferObjectForAttribute"/>
    /// </summary>
    /// <param name="type">The type of the entity represented by the DTO</param>
    /// <exception cref="DomainArgumentException"></exception>
    public DataTransferObjectForAttribute(Type type)
    {
        if (type == null) throw DomainException.ArgumentNullOrWhitespace(nameof(type));
        this.Type = type;
    }

    /// <summary>
    /// Gets the type of the entity represented by the DTO
    /// </summary>
    public Type Type { get; }
}