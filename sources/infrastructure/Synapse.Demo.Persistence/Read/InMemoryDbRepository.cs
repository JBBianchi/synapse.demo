namespace Synapse.Demo.Persistence.Read;

/// <summary>
/// An in memory, transactionless, <see cref="IRepository"/>, used for read models
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public class InMemoryDbRepository<TEntity, TKey> 
    : RepositoryBase<TEntity, TKey>
    , IDisposable
        where TEntity : class, IIdentifiable<TKey> 
        where TKey : IEquatable<TKey>
    
{

    /// <summary>
    /// The <see cref="IRepository"/> storage
    /// </summary>
    protected Dictionary<TKey, TEntity> Data { get; init; } = new Dictionary<TKey, TEntity>();

    /// <summary>
    /// Gets the <see cref="ILogger"/>
    /// </summary>    
    protected ILogger Logger { get; init; }

    /// <summary>
    /// Initializes a new <see cref="InMemoryDbRepository{TEntity, TKey}"/>
    /// </summary>
    /// <param name="logger"></param>
    public InMemoryDbRepository(ILogger<InMemoryDbRepository<TEntity, TKey>> logger)
    {
        this.Logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) throw DomainException.ArgumentNull(nameof(entity));
        if (this.Data.ContainsKey(entity.Id)) throw DomainException.EntityAlreadyExists(typeof(TEntity), entity.Id, "Id");
        this.Data.Add(entity.Id, entity);
        this.Logger.LogTrace($"Added entity with key '{entity.Id}'");
        return await Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public override IQueryable<TEntity> AsQueryable()
    {
        return this.Data.Values.AsQueryable();
    }

    /// <inheritdoc/>
    public override async Task<bool> ContainsAsync(TKey key, CancellationToken cancellationToken = default)
    {
        if (key == null) throw DomainException.ArgumentNull(nameof(key));
        return await Task.FromResult(this.Data.ContainsKey(key));
    }

    /// <inheritdoc/>
    public override async Task<TEntity> FindAsync(TKey key, CancellationToken cancellationToken = default)
    {
        if (key == null) throw DomainException.ArgumentNull(nameof(key));
        if (!this.Data.ContainsKey(key))
        {
            this.Logger.LogTrace($"Unable to find entity with key '{key}'");
            return null;
        }
        this.Logger.LogTrace($"Found entity with key '{key}'");
        return await Task.FromResult(this.Data[key]);
    }

    /// <inheritdoc/>
    public override async Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
    {
        if (keyValues == null
            || keyValues.Length < 1)
            throw DomainException.ArgumentNull(nameof(keyValues));
        return await this.FindAsync((TKey)keyValues.First(), cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<TEntity> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) throw DomainException.ArgumentNull(nameof(entity));
        if (!this.Data.ContainsKey(entity.Id)) throw new DomainException($"Unable to remove the entity with key '{entity.Id}'.");
        this.Data.Remove(entity.Id);
        this.Logger.LogTrace($"Removed entity with key '{entity.Id}'");
        return await Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public override async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override async Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(this.Data.Values.ToList());
    }

    /// <inheritdoc/>
    public override async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) throw DomainException.ArgumentNull(nameof(entity));
        await this.RemoveAsync(entity, cancellationToken);
        await this.AddAsync(entity, cancellationToken);
        this.Logger.LogTrace($"Updated entity with key '{entity.Id}'");
        return await Task.FromResult(entity);
    }


    private bool disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                this.Data.Clear();
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
