using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.Repositories;

/// <summary>
/// Read-only repository
/// </summary>
/// <inheritdoc cref="IReadOnlyMongoDbRepository{TEntity}"/>
public class ReadOnlyMongoDbRepository<TEntity> : IReadOnlyMongoDbRepository<TEntity> where TEntity : SnowflakeEntity
{
    /// <summary>
    /// Current <see cref="Transaction"/>
    /// </summary>
    protected readonly Transaction Transaction;

    internal ReadOnlyMongoDbRepository(Transaction transaction)
    {
        Transaction = transaction;
    }

    /// <inheritdoc />
    public virtual async Task<TEntity?> GetAsync(string id)
        => await DB.Find<TEntity>().OneAsync(id);

    /// <inheritdoc />
    public virtual async Task<long> LongCountAsync()
        => await DB.CountAsync<TEntity>();

    /// <inheritdoc />
    public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        => await DB.CountAsync(expression, null, cancellationToken);

    /// <inheritdoc />
    public virtual async Task<IReadOnlyList<TProjectTo>> GetAllAsync<TProjectTo>() where TProjectTo : class
        => (await DB.Find<TEntity, TProjectTo>().ExecuteAsync()).AsReadOnly();


    /// <inheritdoc />
    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => (await DB.Find<TEntity>().ExecuteAsync()).AsReadOnly();
    
    /// <inheritdoc />
    public virtual Find<TEntity> Find()
        => DB.Find<TEntity>();
    
    /// <inheritdoc />
    public virtual Find<TEntity, TProjection> Find<TProjection>()
        => DB.Find<TEntity, TProjection>();
    
    /// <inheritdoc />
    public virtual Distinct<TEntity, TProperty> Distinct<TProperty>()
        => DB.Distinct<TEntity, TProperty>();
            
    /// <inheritdoc />
    public virtual IAggregateFluent<TEntity> Fluent()
        => DB.Fluent<TEntity>();

    /// <inheritdoc />
    public virtual async Task<long> CountAsync()
        => await DB.CountAsync<TEntity>();
    
    /// <inheritdoc />
    public virtual async Task<long> CountEstimatedAsync()
        => await DB.CountEstimatedAsync<TEntity>();
    
    /// <inheritdoc />
    public virtual async Task<long> CountAsync(FilterDefinition<TEntity> filterDefinition)
        => await DB.CountAsync(filterDefinition);
    
    /// <inheritdoc />
    public virtual async Task<long> CountAsync(Func<FilterDefinitionBuilder<TEntity>, FilterDefinition<TEntity>> filterDefinitionBuilder)
        => await DB.CountAsync(filterDefinitionBuilder);
    
    /// <inheritdoc />
    public virtual PagedSearch<TEntity> PagedSearch()
        => DB.PagedSearch<TEntity>();
    
    /// <inheritdoc />
    public virtual PagedSearch<TEntity, TProjection> PagedSearch<TProjection>()
        => DB.PagedSearch<TEntity, TProjection>();
    
    /// <inheritdoc />
    public virtual async Task<IReadOnlyList<TResult>> PipelineAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false)
        => (await DB.PipelineAsync(template, options, null, cancellationToken)).AsReadOnly();

    /// <inheritdoc />
    public virtual IAggregateFluent<TEntity> FluentTextSearch(Search searchType, string searchTerm,
        bool caseSensitive = false, bool diacriticSensitive = false, string? language = null,
        AggregateOptions? options = null, bool ignoreGlobalFilters = false)
        => DB.FluentTextSearch<TEntity>(searchType, searchTerm, caseSensitive, diacriticSensitive, language,
            options);
    
    /// <inheritdoc />
    public virtual async Task<IAsyncCursor<TResult>> PipelineCursorAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false)
        => await DB.PipelineCursorAsync(template, options, null, cancellationToken);
    
    /// <inheritdoc />
    public virtual async Task<TResult> PipelineFirstAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false)
        => await DB.PipelineFirstAsync(template, options, null, cancellationToken);
    
    /// <inheritdoc />
    public virtual async Task<TResult> PipelineSingleAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false)
        => await DB.PipelineSingleAsync(template, options, null, cancellationToken);
}