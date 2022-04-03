using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.Repositories;

/// <summary>
/// Repository
/// </summary>
/// <typeparam name="TEntity">Entity that derives from <see cref="SnowflakeEntity"/></typeparam>
public interface IMongoDbRepository<TEntity> : IReadOnlyMongoDbRepository<TEntity> where TEntity : SnowflakeEntity
{
    /// <summary>
    /// Adds an entity
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <param name="cancellationToken"></param>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a range of entities
    /// </summary>
    /// <param name="entities">Entities to add</param>
    /// <param name="cancellationToken"></param>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    /// <summary>
    /// Deletes an entity
    /// </summary>
    Task DeleteAsync(Expression<Func<TEntity, bool>> expression);
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of the entity to delete</param>
    Task DeleteAsync(string id);
    /// <summary>
    /// Deletes a range of entities
    /// </summary>
    /// <param name="ids">Ids of the entities to delete</param>
    Task DeleteRangeAsync(IEnumerable<string> ids);
    /// <summary>
    /// Disables an entity
    /// </summary>
    /// <param name="entity">Entity to disable</param>
    Task DisableAsync(TEntity entity);
    /// <summary>
    /// Disables an entity
    /// </summary>
    /// <param name="id">Id of the entity to disable</param>
    Task DisableAsync(string id);
    /// <summary>
    /// Disables a range of entities
    /// </summary>
    /// <param name="entities">Entities to disable</param>
    Task DisableRangeAsync(IEnumerable<TEntity> entities);
    /// <summary>
    /// Disables a range of entities
    /// </summary>
    /// <param name="ids">Ids of the entities to disable</param>
    Task DisableRangeAsync(IEnumerable<string> ids);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Update<TEntity> Update(TEntity entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    UpdateAndGet<TEntity, TEntity> UpdateAndGet(TEntity entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TProjection"></typeparam>
    /// <returns></returns>
    UpdateAndGet<TEntity, TProjection> UpdateAndGet<TProjection>(TEntity entity);
}