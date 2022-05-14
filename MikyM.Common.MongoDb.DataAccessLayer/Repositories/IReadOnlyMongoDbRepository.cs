using System.Collections.Generic;
using System.Threading;
using MikyM.Common.DataAccessLayer.Repositories;
using MongoDB.Driver;
using MongoDB.Entities;
#pragma warning disable CS1574, CS1584, CS1581, CS1580

namespace MikyM.Common.MongoDb.DataAccessLayer.Repositories;

/// <summary>
/// Read-only repository
/// </summary>
/// <typeparam name="TEntity">Entity that derives from <see cref="SnowflakeMongoDbEntity"/></typeparam>
public interface IReadOnlyMongoDbRepository<TEntity> : IBaseRepository where TEntity : SnowflakeMongoDbEntity
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
    /// Gets an entity based on given id and projects it to another type
    /// </summary>
    /// <param name="id">Id of the entity</param>
    /// <typeparam name="TProjectTo">Type to which the entity should be projected</typeparam>
    /// <returns>Entity if found, null if not found</returns>
    Task<TProjectTo?> GetAsync<TProjectTo>(string id);

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

    /// <inheritdoc cref="DB.Find{TEntity}"/>
    Find<TEntity> Find();

    /// <inheritdoc cref="DB.Find{TEntity, TProjection}"/>
    Find<TEntity, TProjection> Find<TProjection>();

    /// <inheritdoc cref="DB.Distinct{TEntity, TProperty}"/>
    Distinct<TEntity, TProperty> Distinct<TProperty>();

    /// <inheritdoc cref="DB.Fluent{TEntity}"/>
    IAggregateFluent<TEntity> Fluent();

    /// <summary>
    /// Retrieves the count of the entities
    /// </summary>
    /// <returns>Count of the entities</returns>
    Task<long> CountAsync();

    /// <inheritdoc cref="DB.CountEstimatedAsync{TEntity}"/>
    Task<long> CountEstimatedAsync();

    /// <inheritdoc cref="DB.CountAsync{TEntity}(FilterDefinition{TEntity})"/>
    Task<long> CountAsync(FilterDefinition<TEntity> filterDefinition);

    /// <inheritdoc cref="DB.CountAsync(Func{FilterDefinitionBuilder{TEntity}}, FilterDefinition{TEntity})"/>
    Task<long> CountAsync(
        Func<FilterDefinitionBuilder<TEntity>, FilterDefinition<TEntity>> filterDefinitionBuilder);

    /// <inheritdoc cref="DB.PagedSearch{TProjection}"/>
    PagedSearch<TEntity> PagedSearch();

    /// <inheritdoc cref="DB.PagedSearch{TProjection}"/>
    PagedSearch<TEntity, TProjection> PagedSearch<TProjection>();

    /// <inheritdoc cref="DB.PipelineAsync{TResult}"/>
    Task<IReadOnlyList<TResult>> PipelineAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false);

    /// <inheritdoc cref="DB.FluentTextSearch{TEntity}"/>
    IAggregateFluent<TEntity> FluentTextSearch(Search searchType, string searchTerm,
        bool caseSensitive = false, bool diacriticSensitive = false, string? language = null,
        AggregateOptions? options = null, bool ignoreGlobalFilters = false);

    /// <inheritdoc cref="DB.PipelineCursorAsync{TResult}"/>
    Task<IAsyncCursor<TResult>> PipelineCursorAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false);

    /// <inheritdoc cref="DB.PipelineFirstAsync{TResult}"/>
    Task<TResult> PipelineFirstAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false);

    /// <inheritdoc cref="DB.PipelineSingleAsync{TResult}"/>
    Task<TResult> PipelineSingleAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false);
}