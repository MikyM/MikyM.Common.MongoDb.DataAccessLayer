using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using MikyM.Common.MongoDb.DataAccessLayer.Context;
using MongoDB.Driver;
using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.Repositories;

/// <summary>
/// Read-only repository
/// </summary>
/// <inheritdoc cref="IReadOnlyMongoDbRepository{TEntity}"/>
public class ReadOnlyMongoDbRepository<TEntity> : IReadOnlyMongoDbRepository<TEntity> where TEntity : SnowflakeMongoDbEntity
{
    /// <inheritdoc />
    public Type EntityType => typeof(TEntity);

    /// <summary>
    /// Current <see cref="MongoDbContext"/>
    /// </summary>
    protected readonly MongoDbContext Context;

    internal ReadOnlyMongoDbRepository(MongoDbContext context)
    {
        Context = context;
    }

    /// <inheritdoc />
    public virtual async Task<TEntity?> GetAsync(string id)
        => await Context.Transaction.Find<TEntity, TEntity>().OneAsync(id);
    
    /// <inheritdoc />
    public virtual async Task<TProjectTo?> GetAsync<TProjectTo>(string id)
        => await Context.Transaction.Find<TEntity, TProjectTo>().OneAsync(id);

    /// <inheritdoc />
    public virtual async Task<long> LongCountAsync()
        => await Context.Transaction.CountAsync<TEntity>();

    /// <inheritdoc />
    public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        => await Context.Transaction.CountAsync(expression, cancellationToken);

    /// <inheritdoc />
    public virtual async Task<IReadOnlyList<TProjectTo>> GetAllAsync<TProjectTo>() where TProjectTo : class
        => (await Context.Transaction.Find<TEntity, TProjectTo>().ExecuteAsync()).AsReadOnly();


    /// <inheritdoc />
    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => (await Context.Transaction.Find<TEntity>().ExecuteAsync()).AsReadOnly();
    
    /// <inheritdoc />
    public virtual Find<TEntity> Find()
        => Context.Transaction.Find<TEntity>();
    
    /// <inheritdoc />
    public virtual Find<TEntity, TProjection> Find<TProjection>()
        => Context.Transaction.Find<TEntity, TProjection>();
    
    /// <inheritdoc />
    public virtual Distinct<TEntity, TProperty> Distinct<TProperty>()
        => Context.Transaction.Distinct<TEntity, TProperty>();
            
    /// <inheritdoc />
    public virtual IAggregateFluent<TEntity> Fluent()
        => Context.Transaction.Fluent<TEntity>();

    /// <inheritdoc />
    public virtual async Task<long> CountAsync()
        => await Context.Transaction.CountAsync<TEntity>();
    
    /// <inheritdoc />
    public virtual async Task<long> CountEstimatedAsync()
        => await Context.Transaction.CountEstimatedAsync<TEntity>();
    
    /// <inheritdoc />
    public virtual async Task<long> CountAsync(FilterDefinition<TEntity> filterDefinition)
        => await Context.Transaction.CountAsync(filterDefinition);
    
    /// <inheritdoc />
    public virtual async Task<long> CountAsync(Func<FilterDefinitionBuilder<TEntity>, FilterDefinition<TEntity>> filterDefinitionBuilder)
        => await Context.Transaction.CountAsync(filterDefinitionBuilder);
    
    /// <inheritdoc />
    public virtual PagedSearch<TEntity> PagedSearch()
        => Context.Transaction.PagedSearch<TEntity>();
    
    /// <inheritdoc />
    public virtual PagedSearch<TEntity, TProjection> PagedSearch<TProjection>()
        => Context.Transaction.PagedSearch<TEntity, TProjection>();
    
    /// <inheritdoc />
    public virtual async Task<IReadOnlyList<TResult>> PipelineAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false)
        => (await Context.Transaction.PipelineAsync(template, options,  cancellationToken)).AsReadOnly();

    /// <inheritdoc />
    public virtual IAggregateFluent<TEntity> FluentTextSearch(Search searchType, string searchTerm,
        bool caseSensitive = false, bool diacriticSensitive = false, string? language = null,
        AggregateOptions? options = null, bool ignoreGlobalFilters = false)
        => Context.Transaction.FluentTextSearch<TEntity>(searchType, searchTerm, caseSensitive, diacriticSensitive, language,
            options);
    
    /// <inheritdoc />
    public virtual async Task<IAsyncCursor<TResult>> PipelineCursorAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false)
        => await Context.Transaction.PipelineCursorAsync(template, options, cancellationToken);
    
    /// <inheritdoc />
    public virtual async Task<TResult> PipelineFirstAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false)
        => await Context.Transaction.PipelineFirstAsync(template, options, cancellationToken);
    
    /// <inheritdoc />
    public virtual async Task<TResult> PipelineSingleAsync<TResult>(Template<TEntity, TResult> template,
        AggregateOptions? options = null, CancellationToken cancellationToken = default,
        bool ignoreGlobalFilters = false)
        => await Context.Transaction.PipelineSingleAsync(template, options, cancellationToken);
}