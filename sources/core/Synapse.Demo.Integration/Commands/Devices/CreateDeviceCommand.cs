namespace Synapse.Demo.Integration.Commands.Devices;

// TODO: Write tests
/// <summary>
/// Represents the command DTO used to create a new <see cref="Device"/>
/// </summary>
[CloudEventEnvelope("device", "create")]
public class CreateDeviceCommand
{
    /// <summary>
    /// Gets the id of the <see cref="Device"/> to create
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets the label of the <see cref="Device"/> to create
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// Gets the type of <see cref="Device"/> to create
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// Gets the location of the <see cref="Device"/> to create
    /// </summary>
    public string Location { get; init; }

    /// <summary>
    /// Gets the state of the <see cref="Device"/> to create
    /// </summary>
    public object? State { get; init; }

    /// <summary>
    /// Initializes a new <see cref="CreateDeviceCommand"></see>
    /// </summary>
    /// <param name="id">The id of the <see cref="Device"/> to create</param>
    /// <param name="label">The label of the <see cref="Device"/> to create</param>
    /// <param name="type">The type of <see cref="Device"/> to create</param>
    /// <param name="location">The location of the <see cref="Device"/> to create</param>
    /// <param name="state">The state of the <see cref="Device"/> to create</param>
    public CreateDeviceCommand(string id, string label, string type, string location, object? state)
    {
        if (string.IsNullOrWhiteSpace(id)) throw DomainException.ArgumentNullOrWhitespace(nameof(id));
        if (string.IsNullOrWhiteSpace(label)) throw DomainException.ArgumentNullOrWhitespace(nameof(label));
        if (string.IsNullOrWhiteSpace(type)) throw DomainException.ArgumentNullOrWhitespace(nameof(type));
        if (string.IsNullOrWhiteSpace(location)) throw DomainException.ArgumentNullOrWhitespace(nameof(location));
        this.Id = id;
        this.Label = label;
        this.Type = type;
        this.Location = location;
        this.State = state;
    }
}
