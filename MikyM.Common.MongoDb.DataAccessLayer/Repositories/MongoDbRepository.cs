using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.Repositories;

/// <summary>
/// Repository
/// </summary>
/// <inheritdoc cref="IMongoDbRepository{TEntity}"/>
public class MongoDbRepository<TEntity> : ReadOnlyMongoDbRepository<TEntity>, IMongoDbRepository<TEntity>
    where TEntity : SnowflakeMongoDbEntity
{
    internal MongoDbRepository(Transaction transaction) : base(transaction)
    {
    }

    /// <inheritdoc />
    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await Transaction.InsertAsync(entity, cancellationToken);

    /// <inheritdoc />
    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => await Transaction.InsertAsync(entities, cancellationToken);

    /// <inheritdoc />
    public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> expression)
        => await Transaction.DeleteAsync(expression);
    
    /// <inheritdoc />
    public virtual async Task DeleteAsync(string id)
        => await Transaction.DeleteAsync<TEntity>(id);

    /// <inheritdoc />
    public virtual async Task DeleteRangeAsync(IEnumerable<string> ids)
        => await Transaction.DeleteAsync<TEntity>(ids);

    /// <inheritdoc />
    public virtual async Task DisableAsync(TEntity entity)
        => await DisableAsync(entity.ID);

    /// <inheritdoc />
    public virtual Update<TEntity> Update(TEntity entity)
        => Transaction.Update<TEntity>();

    /// <inheritdoc />
    public virtual UpdateAndGet<TEntity, TEntity> UpdateAndGet(TEntity entity)
        => Transaction.UpdateAndGet<TEntity>();
    
    /// <inheritdoc />
    public virtual UpdateAndGet<TEntity, TProjection> UpdateAndGet<TProjection>(TEntity entity)
        => Transaction.UpdateAndGet<TEntity, TProjection>();
    
    /// <inheritdoc />
    public virtual async Task DisableAsync(string id)
        => await Transaction.Update<TEntity>().Match(x => x.ID == id).Modify(x => x.Set(y => y.IsDisabled, true))
            .ExecuteAsync();


    /// <inheritdoc />
    public virtual async Task DisableRangeAsync(IEnumerable<TEntity> entities)
        => await DisableRangeAsync(entities.Select(x => x.ID));

    /// <inheritdoc />
    public virtual async Task DisableRangeAsync(IEnumerable<string> ids)
        => await Transaction.Update<TEntity>().Match(x => ids.Contains(x.ID)).Modify(x => x.Set(y => y.IsDisabled, true))
            .ExecuteAsync();
}