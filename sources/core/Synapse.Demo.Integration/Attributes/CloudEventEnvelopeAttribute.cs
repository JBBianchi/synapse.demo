namespace Synapse.Demo.Integration.Attributes;

// TODO: Write tests
/// <summary>
/// Represents an <see cref="Attribute"/> used to configure the <see cref="CloudEvent"/> envelope of the marked <see cref="IntegrationEvent"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CloudEventEnvelopeAttribute
    : Attribute
{

    /// <summary>
    /// Gets the domain aggregate type to build the <see cref="CloudEvent"/> for
    /// </summary>
    public virtual string AggregateType { get; set; }

    /// <summary>
    /// Gets the action name to build the <see cref="CloudEvent"/> for
    /// </summary>
    public virtual string ActionName { get; set; }

    /// <summary>
    /// Gets the <see cref="CloudEvent"/> data schema URI
    /// </summary>
    public virtual string? DataSchemaUri { get; set; }

    /// <summary>
    /// Initializes a new <see cref="CloudEventEnvelopeAttribute"/>
    /// </summary>
    /// <param name="aggregateType">The cloud event type</param>
    /// <param name="actionName">The cloud event type</param>
    /// <param name="dataSchemaUri">The cloud event data schema URI</param>
    public CloudEventEnvelopeAttribute(string aggregateType, string actionName, string? dataSchemaUri = null)
    {
        if (string.IsNullOrWhiteSpace(aggregateType))
            throw new ArgumentNullException(nameof(aggregateType));
        if (string.IsNullOrWhiteSpace(actionName))
            throw new ArgumentNullException(nameof(actionName));
        this.AggregateType = aggregateType;
        this.ActionName = actionName;
        this.DataSchemaUri = dataSchemaUri;
    }
}
