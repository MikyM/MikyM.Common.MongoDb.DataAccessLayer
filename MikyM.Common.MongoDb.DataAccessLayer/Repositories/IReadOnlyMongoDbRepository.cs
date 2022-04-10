using System.Collections.Generic;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.Repositories;

/// <summary>
/// Read-only repository
/// </summary>
/// <typeparam name="TEntity">Entity that derives from <see cref="SnowflakeEntity"/></typeparam>
public interface IReadOnlyMongoDbRepository<TEntity> : IBaseRepository where TEntity : SnowflakeEntity
{
    /// <summary>
    /// Entity type that this repository was created for
    /// </summary>
    Type EntityType { get; }
    
    /// <summary>
    /// Gets an entity based on given id
    /// </summary>
    /// <param name="id">Id of the entity</param>
    /// <returns>Entity if found, null if not found</returns>
    Task<TEntity?> GetAsync(string id);

    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <returns><see cref="IReadOnlyList{T}"/> with all entities</returns>
    Task<IReadOnlyList<TEntity>> GetAllAsync();

    /// <summary>
    /// Gets all entities and projects them to another entity
    /// </summary>
    /// <returns><see cref="IReadOnlyList{T}"/> with all entities</returns>
    Task<IReadOnlyList<TProjectTo>> GetAllAsync<TProjectTo>() where TProjectTo : class;

    /// <inheritdoc />
    Find<TEntity> Find();

    /// <inheritdoc />
    Find<TEntity, TProjection> Find<TProjection>();

    /// <inheritdoc />
    Distinct<TEntity, TProperty> Distinct<TProperty>();

    /// <inheritdoc />
    IAggregateFluent<TEntity> Fluent();

    /// <inheritdoc />
    Task<long> CountAsync();

    /// <inheritdoc />
    Task<long> CountEstimatedAsync();

    /// <inheritdoc />
    Task<long> CountAsync(FilterDefinition<TEntity> filterDefinition);

    /// <inheritdoc />
    Task<long> CountAsync(
        Func<FilterDefinitionBuilder<TEntity>, FilterDefinition<TEntity>> filterDefinitionBuilder);

    /// <inheritdoc />
    PagedSearch<TEntity> PagedSearch();

    /// <inheritdoc />
    PagedSearch<TEntity, TProjection> PagedSearch<TProjection>();

    /// <inheritdoc />
    Task<IReadOnlyList<TResult>> PipelineAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false);

    /// <inheritdoc />
    IAggregateFluent<TEntity> FluentTextSearch(Search searchType, string searchTerm,
        bool caseSensitive = false, bool diacriticSensitive = false, string? language = null,
        AggregateOptions? options = null, bool ignoreGlobalFilters = false);

    /// <inheritdoc />
    Task<IAsyncCursor<TResult>> PipelineCursorAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false);

    /// <inheritdoc />
    Task<TResult> PipelineFirstAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false);

    /// <inheritdoc />
    Task<TResult> PipelineSingleAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false);
}