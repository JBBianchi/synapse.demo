namespace Synapse.Demo.Persistence.Write;

/// <summary>
/// An in memory <see cref="IEventStore"/>, used for event sourcing
/// </summary>
public class InMemoryEventStore
    : IEventStore
{
    /// <summary>
    /// Gets the <see cref="ILogger"/>
    /// </summary>    
    protected ILogger Logger { get; init; }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected virtual IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the <see cref="IEventStream"/>s storage
    /// </summary>
    protected Dictionary<string, InMemoryEventSource> Streams { get; init; } = new Dictionary<string, InMemoryEventSource>();

    /// <summary>
    /// Initializes a new <see cref="InMemoryEventStore"/>
    /// </summary>
    /// <param name="logger">The service used to perform logging</param>
    /// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
    public InMemoryEventStore(ILogger<InMemoryEventStore> logger, IServiceProvider serviceProvider)
    {
        this.Logger = logger;
        this.ServiceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<bool> StreamExistsAsync(string streamId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        return await Task.FromResult(this.Streams.ContainsKey(streamId));
    }

    /// <inheritdoc/>
    public virtual async Task<IEventStream> GetStreamAsync(string streamId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) return null!;
        return await this.Streams[streamId].GetStreamAsync(this, streamId, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task AppendToStreamAsync(string streamId, IEnumerable<IEventMetadata> events, long expectedVersion, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (events == null || !events.Any()) throw DomainException.ArgumentNull(nameof(events));
        if (!await this.StreamExistsAsync(streamId, cancellationToken))
        {
            this.Streams.Add(streamId, ActivatorUtilities.CreateInstance<InMemoryEventSource>(this.ServiceProvider));
        }
        await this.Streams[streamId].AppendToStreamAsync(events, expectedVersion, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task AppendToStreamAsync(string streamId, IEnumerable<IEventMetadata> events, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (events == null || !events.Any()) throw DomainException.ArgumentNull(nameof(events));
        if (!await this.StreamExistsAsync(streamId, cancellationToken))
        {
            this.Streams.Add(streamId, ActivatorUtilities.CreateInstance<InMemoryEventSource>(this.ServiceProvider));
        }
        await this.Streams[streamId].AppendToStreamAsync(events, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ISourcedEvent>> ReadEventsForwardAsync(string streamId, long offset, long length, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (offset < 0) throw DomainException.ArgumentMustBeHigherOrEqualTo(nameof(offset), 0);
        if (length < 0) throw DomainException.ArgumentMustBeHigherOrEqualTo(nameof(length), 0);
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) return null!;
        return await this.Streams[streamId].ReadEventsForwardAsync(offset, length, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ISourcedEvent>> ReadEventsForwardAsync(string streamId, long offset, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (offset < 0) throw DomainException.ArgumentMustBeHigherOrEqualTo(nameof(offset), 0);
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) return null!;
        return await this.Streams[streamId].ReadEventsForwardAsync(offset, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ISourcedEvent>> ReadAllEventsForwardAsync(string streamId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) return null!;
        return await this.Streams[streamId].ReadAllEventsForwardAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<ISourcedEvent> ReadSingleEventForwardAsync(string streamId, long offset, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (offset < 0) throw DomainException.ArgumentMustBeHigherOrEqualTo(nameof(offset), 0);
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) return null!;
        return await this.Streams[streamId].ReadSingleEventForwardAsync(offset, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ISourcedEvent>> ReadEventsBackwardAsync(string streamId, long offset, long length, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (offset < 0) throw DomainException.ArgumentMustBeHigherOrEqualTo(nameof(offset), 0);
        if (length < 0) throw DomainException.ArgumentMustBeHigherOrEqualTo(nameof(length), 0);
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) return null!;
        return await this.Streams[streamId].ReadEventsBackwardAsync(offset, length, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ISourcedEvent>> ReadEventsBackwardAsync(string streamId, long offset, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (offset < 0) throw DomainException.ArgumentMustBeHigherOrEqualTo(nameof(offset), 0);
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) return null!;
        return await this.Streams[streamId].ReadEventsBackwardAsync(offset, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<ISourcedEvent> ReadSingleEventBackwardAsync(string streamId, long offset, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (offset < 0) throw DomainException.ArgumentMustBeHigherOrEqualTo(nameof(offset), 0);
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) return null!;
        return await this.Streams[streamId].ReadSingleEventBackwardAsync(offset, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<ISourcedEvent>> ReadAllEventsBackwardAsync(string streamId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) return null!;
        return await this.Streams[streamId].ReadAllEventsBackwardAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<string> SubscribeToStreamAsync(string streamId, Action<ISubscriptionOptionsBuilder> setup, Func<IServiceProvider, ISourcedEvent, Task> handler, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual async Task<string> SubscribeToStreamAsync(string streamId, Func<IServiceProvider, ISourcedEvent, Task> handler, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual void UnsubscribeFrom(string subscriptionId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual async Task TruncateStreamAsync(string streamId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual async Task TruncateStreamAsync(string streamId, long beforeVersion, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual async Task DeleteStreamAsync(string streamId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(streamId)) throw DomainException.ArgumentNull(nameof(streamId));
        if (!await this.StreamExistsAsync(streamId, cancellationToken)) throw new DomainException($"Unable to find a stream with id '{streamId}'");
        await this.Streams[streamId].DeleteStreamAsync(cancellationToken);
    }

}
