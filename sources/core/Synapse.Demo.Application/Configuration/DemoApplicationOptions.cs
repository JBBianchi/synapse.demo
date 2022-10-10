namespace Synapse.Demo.Application.Configuration;

/// <summary>
/// Represents the application configuration options
/// </summary>
public class DemoApplicationOptions
    : IDemoApplicationOptions
{
    /// <inheritdoc/>
    public string CloudEventsSource { get; set; } = String.Empty;

    /// <inheritdoc/>
    public string SchemaRegistry { get; set; } = null!;

    /// <inheritdoc/>
    public string CloudEventBroker { get; set; } = null!;

    /// <summary>
    /// Initialises a new <see cref="DemoApplicationOptions"/>
    /// </summary>
    public DemoApplicationOptions()
    { }
}
