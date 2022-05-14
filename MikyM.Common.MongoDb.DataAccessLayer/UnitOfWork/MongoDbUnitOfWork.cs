using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Options;
using MikyM.Common.DataAccessLayer.Repositories;
using MikyM.Common.MongoDb.DataAccessLayer.Context;
using MikyM.Common.MongoDb.DataAccessLayer.Helpers;
using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;

/// <summary>
/// Unit of work implementation
/// </summary>
/// <inheritdoc cref="IMongoDbUnitOfWork"/>
public sealed class MongoDbUnitOfWork<TContext> : IMongoDbUnitOfWork<TContext> where TContext : MongoDbContext
{
    // To detect redundant calls
    private bool _disposed;
    // ReSharper disable once InconsistentNaming

    private readonly IOptions<MongoDbDataAccessConfiguration> _options;

    /// <summary>
    /// Repository cache
    /// </summary>
    private ConcurrentDictionary<string, IRepositoryBase>? _repositories;
    /// <summary>
    /// Repository entity type cache
    /// </summary>
    private ConcurrentDictionary<string, string>? _entityTypesOfRepositories;
    
    /// <inheritdoc/>
    public TContext Context { get; }

    /// <summary>
    /// Creates a new instance of <see cref="MongoDbUnitOfWork{TContext}"/>
    /// </summary>
    public MongoDbUnitOfWork(TContext context,IOptions<MongoDbDataAccessConfiguration> options)
    {
        Context = context;
        _options = options;
    }

    /// <inheritdoc />
    public TRepository GetRepository<TRepository>() where TRepository : class, IRepositoryBase
    {
        _repositories ??= new ConcurrentDictionary<string, IRepositoryBase>();
        _entityTypesOfRepositories ??= new ConcurrentDictionary<string, string>();

        var type = typeof(TRepository);
        string name = type.FullName ?? throw new InvalidOperationException();
        var entityType = type.GetGenericArguments().FirstOrDefault();
        if (entityType is null)
            throw new ArgumentException("Couldn't retrieve entity type from generic arguments on repository type");

        if (type.IsInterface)
        {
            if (!UoFCache.CachedRepositoryInterfaceImplTypes.TryGetValue(type, out var implType))
                throw new InvalidOperationException($"Couldn't find a non-abstract implementation of {name}");

            type = implType;
            name = implType.FullName ?? throw new InvalidOperationException();
        }
        
        if (_repositories.TryGetValue(name, out var repository)) 
            return (TRepository)repository;

        if (_entityTypesOfRepositories.TryGetValue(entityType.Name, out _))
            throw new InvalidOperationException(
                "Seems like you tried to create a different type of repository (ie. both read-only and crud) for same entity type within same unit of work instance - it is not supported as it may lead to unexpected results");

        var instance = Activator.CreateInstance(type,
            BindingFlags.NonPublic | BindingFlags.Instance, null, new object[]
            {
                Context
            }, CultureInfo.InvariantCulture);

        if (instance is null) throw new InvalidOperationException($"Couldn't create an instance of {name}");

        var castInstance = (TRepository)instance;
        
        if (_repositories.TryAdd(name, castInstance))
        {
            _entityTypesOfRepositories.TryAdd(entityType.Name, entityType.Name);
            return (TRepository)_repositories[name];
        }

        if (_repositories.TryGetValue(name, out repository)) 
            return (TRepository)repository;

        throw new InvalidOperationException(
            $"Repository of type {name} couldn't be added to and/or retrieved.");
    }

    /// <inheritdoc />
    public async Task RollbackAsync()
        => await Context.RollbackAsync();

    /// <inheritdoc />
    public async Task CommitAsync()
        => await Context.CommitAsync();

    /// <inheritdoc />
    public async Task CommitAsync(string userId)
        => await Context.CommitAsync(userId);

    // Public implementation of Dispose pattern callable by consumers.
    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            Context.Dispose();
        }

        _repositories = null;
        _entityTypesOfRepositories = null;

        _disposed = true;
    }
}