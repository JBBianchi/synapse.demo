namespace Synapse.Demo.Persistence.Write;

public class InMemoryEvent
    : IEventMetadata
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public long Sequence { get; set; }

    public string Type { get; set; }

    public object Data { get; set; }

    public object Metadata { get; set; }

    public InMemoryEvent(Guid id, DateTimeOffset createdAt, long sequence, string type, object data, object metadata)
    {
        this.Id = id;
        this.CreatedAt = createdAt;
        this.Sequence = sequence;
        this.Type = type;
        this.Data = data;
        this.Metadata = metadata;
    }
}
