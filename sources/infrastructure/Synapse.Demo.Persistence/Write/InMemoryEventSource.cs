namespace Synapse.Demo.Persistence.Write;

public class InMemoryEventSource
{
    /// <summary>
    /// Gets the <see cref="ILogger"/>
    /// </summary>    
    protected ILogger Logger { get; init; }

    /// <summary>
    /// Gets the pile of <see cref="IEventMetadata"/>s
    /// </summary>
    protected HashSet<InMemoryEvent> Events { get; init; }

    /// <summary>
    /// Gets the <see cref="ReplaySubject"/> of  <see cref="IEventMetadata"/>s
    /// </summary>
    protected ReplaySubject<InMemoryEvent> EventsStream { get; init; }

    /// <summary>
    /// The last index
    /// </summary>
    private long _sequence = -1;

    /// <summary>
    /// Initliazses a new <see cref="InMemoryEventSource"/>
    /// </summary>
    /// <param name="logger"></param>
    public InMemoryEventSource(ILogger<InMemoryEventSource> logger)
    {
        this.Logger = logger;
        this.Events = new HashSet<InMemoryEvent>();
        this.EventsStream = new ReplaySubject<InMemoryEvent>(TimeSpan.MaxValue);
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task<IEventStream> GetStreamAsync(IEventStore eventStore, string streamId, CancellationToken cancellationToken = default)
    {
        var lastEvent = this.UnwrapMetadata(this.Events.Last());
        return await Task.FromResult(new EventStream(eventStore, streamId, this.Events.Count, this.Events.First().CreatedAt.DateTime, lastEvent.CreatedAt.DateTime, lastEvent));
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task<IEnumerable<ISourcedEvent>> ReadAllEventsForwardAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(
            this.Events
                .Select(this.UnwrapMetadata)
        );
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task<IEnumerable<ISourcedEvent>> ReadEventsForwardAsync(long offset, long length, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(
            this.Events
                .Skip((int)offset)
                .Take((int)length)
                .Select(this.UnwrapMetadata)
        );
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task<IEnumerable<ISourcedEvent>> ReadEventsForwardAsync(long offset, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(
            this.Events
                .Skip((int)offset)
                .Select(this.UnwrapMetadata)
        );
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task<ISourcedEvent> ReadSingleEventForwardAsync(long offset, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(
            this.Events
                .Skip((int)offset)
                .Select(this.UnwrapMetadata)
                .FirstOrDefault()!
        );
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task<IEnumerable<ISourcedEvent>> ReadEventsBackwardAsync(long offset, long length, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(
            this.Events
                .Reverse()
                .Skip((int)offset)
                .Take((int)length)
                .Select(this.UnwrapMetadata)
        );
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task<IEnumerable<ISourcedEvent>> ReadEventsBackwardAsync(long offset, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(
            this.Events
                .Reverse()
                .Skip((int)offset)
                .Select(this.UnwrapMetadata)
        );
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task<ISourcedEvent> ReadSingleEventBackwardAsync(long offset, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(
            this.Events
                .Reverse()
                .Skip((int)offset)
                .Select(this.UnwrapMetadata)
                .FirstOrDefault()!
        );
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task<IEnumerable<ISourcedEvent>> ReadAllEventsBackwardAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(
            this.Events
                .Reverse()
                .Select(this.UnwrapMetadata)
        );
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task AppendToStreamAsync(IEnumerable<IEventMetadata> events, CancellationToken cancellationToken = default)
    {
        await this.AppendToStreamAsync(events, 0, cancellationToken);
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public async Task AppendToStreamAsync(IEnumerable<IEventMetadata> events, long expectedVersion, CancellationToken cancellationToken = default)
    {
        if (expectedVersion != this._sequence+1) throw new DomainException($"The expected version '{expectedVersion}' doesn't match the current state version '{this._sequence}'.");
        foreach (var e in events)
        {
            this._sequence++;
            var inMemoryEvent = new InMemoryEvent(e.Id, DateTimeOffset.UtcNow, this._sequence, e.Type, e.Data, e.Metadata);
            this.Events.Add(inMemoryEvent);
            this.EventsStream.OnNext(inMemoryEvent);
        }
        await Task.CompletedTask;
    }

    /// <summary>
    /// See <see cref="IEventStore"/>
    /// </summary>
    public virtual async Task DeleteStreamAsync(CancellationToken cancellationToken = default)
    {
        this.Events.Clear();
        await Task.CompletedTask;
    }

    /// <summary>
    /// Unwraps a <see cref="InMemoryEvent"/> as <see cref="ISourcedEvent"/>
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected ISourcedEvent UnwrapMetadata(InMemoryEvent e) => new SourcedEvent(e.Id, e.Sequence, e.CreatedAt, e.Type, e.Data, e.Metadata);
}
