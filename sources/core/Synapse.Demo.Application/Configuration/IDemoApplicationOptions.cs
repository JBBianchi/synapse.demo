namespace Synapse.Demo.Application.Configuration;

/// <summary>
/// Represents the application configuration options
/// </summary>
public interface IDemoApplicationOptions 
{
    /// <summary>
    /// Gets/Sets the sources of the <see cref="CloudEvent"/>s emitted by the application
    /// </summary>
    string CloudEventsSource { get; set; }

    /// <summary>
    /// Gets/Sets the schema registry holding the <see cref="CloudEvent"/>s data schemas
    /// </summary>
    string SchemaRegistry { get; set; }

    /// <summary>
    /// Gets/Sets the <see cref="CloudEvent"/>s broker
    /// </summary>
    string CloudEventBroker { get; set; }
}
