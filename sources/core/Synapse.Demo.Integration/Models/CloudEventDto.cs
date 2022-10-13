namespace Synapse.Demo.Integration.Models;

/// <summary>
/// The DTO of a cloud event
/// </summary>
public class CloudEventDto
{
    /// <summary>
    /// CloudEvent <see href="https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#id">'id'</see> attribute,
    /// This is the ID of the event. When combined with <see cref="Source"/>, this enables deduplication.
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// CloudEvents <see href="https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#source">'source'</see> attribute.
    /// This describes the event producer. Often this will include information such as the type of the event source, the
    /// organization publishing the event, the process that produced the event, and some unique identifiers.
    /// When combined with <see cref="Id"/>, this enables deduplication.
    /// </summary>
    public Uri Source { get; set; } = null!;

    /// <summary>
    /// CloudEvents <see href="https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#type">'type'</see> attribute.
    /// Type of occurrence which has happened.
    /// Often this attribute is used for routing, observability, policy enforcement, etc.
    /// </summary>
    public string Type { get; set; } = null!;

    /// <summary>
    /// CloudEvents <see href="https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#subject">'subject'</see> attribute.
    /// This describes the subject of the event in the context of the event producer (identified by <see cref="Source"/>).
    /// In publish-subscribe scenarios, a subscriber will typically subscribe to events emitted by a source,
    /// but the source identifier alone might not be sufficient as a qualifier for any specific event if the source context has
    /// internal sub-structure.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// CloudEvents <see href="https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#time">'time'</see> attribute.
    /// Timestamp of when the occurrence happened.
    /// </summary>
    public DateTimeOffset? Time { get; set; }

    /// <summary>
    /// CloudEvents <see href="https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#dataschema">'dataschema'</see> attribute.
    /// A link to the schema that the data attribute adheres to.
    /// Incompatible changes to the schema SHOULD be reflected by a different URI.
    /// </summary>
    public Uri? DataSchema { get; set; }

    /// <summary>
    /// CloudEvent 'data' content.  The event payload. The payload depends on the type
    /// and the 'schemaurl'. It is encoded into a media format which is specified by the
    /// 'contenttype' attribute (e.g. application/json).
    /// </summary>
    /// <see href="https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#event-data"/>
    public object? Data { get; set; }

    /// <summary>
    /// Initializes a new <see cref="CloudEventDto"/>
    /// </summary>
    /// <param name="id"><see cref="Id"/></param>
    /// <param name="source"><see cref="Source"/></param>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="data"><see cref="Data"/></param>
    /// <param name="subject"><see cref="Subject"/></param>
    /// <param name="time"><see cref="Time"/></param>
    /// <param name="dataSchema"><see cref="DataSchema"/></param>
    public CloudEventDto(string id, string source, string type, object? data = null, string? subject = null, DateTimeOffset? time = null, Uri? dataSchema = null)
    {
        if (string.IsNullOrWhiteSpace(id)) throw DomainException.ArgumentNullOrWhitespace(nameof(id));
        if (string.IsNullOrWhiteSpace(source)) throw DomainException.ArgumentNullOrWhitespace(nameof(source));
        if (string.IsNullOrWhiteSpace(type)) throw DomainException.ArgumentNullOrWhitespace(nameof(type));
        this.Id = id;
        this.Source = new (source, UriKind.RelativeOrAbsolute);
        this.Type = type;
        this.Subject = subject;
        this.Time = time;
        this.DataSchema = dataSchema;
        this.Data = data;
    }

    /// <summary>
    /// Initializes a new <see cref="CloudEventDto"/>
    /// </summary>
    protected CloudEventDto() { }
}
