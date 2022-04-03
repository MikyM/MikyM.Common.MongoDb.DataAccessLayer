using System.Collections.Concurrent;
using Autofac;
using Autofac.Core;
using MikyM.Common.MongoDb.DataAccessLayer.Repositories;
using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;

/// <summary>
/// Unit of work implementation
/// </summary>
/// <inheritdoc cref="IMongoDbUnitOfWork"/>
public sealed class MongoDbUnitOfWork : IMongoDbUnitOfWork
{
    /// <summary>
    /// Inner root <see cref="ILifetimeScope"/>
    /// </summary>
    private readonly ILifetimeScope _lifetimeScope;

    // To detect redundant calls
    private bool _disposed;
    // ReSharper disable once InconsistentNaming

    /// <summary>
    /// Repository cache
    /// </summary>
    private ConcurrentDictionary<string, IBaseRepository>? _repositories;

    /// <summary>
    /// Inner <see cref="Transaction"/>
    /// </summary>
    private Transaction _transaction;

    /// <summary>
    /// Creates a new instance of <see cref="MongoDbUnitOfWork"/>
    /// </summary>
    /// <param name="lifetimeScope">Root <see cref="ILifetimeScope"/></param>
    public MongoDbUnitOfWork(ILifetimeScope lifetimeScope)
    {
        _transaction = DB.Transaction();
        _lifetimeScope = lifetimeScope;
    }

    /// <inheritdoc />
    public TRepository GetRepository<TRepository>() where TRepository : class, IBaseRepository
    {
        _repositories ??= new ConcurrentDictionary<string, IBaseRepository>();

        var type = typeof(TRepository);
        string name = type.FullName ?? throw new InvalidOperationException();

        if (_repositories.TryGetValue(name, out var repository)) return (TRepository) repository;

        if (_repositories.TryAdd(name,
                _lifetimeScope.Resolve<TRepository>(new ResolvedParameter(
                    (pi, _) => pi.ParameterType.IsAssignableTo(typeof(Transaction)), (_, _) => _transaction))))
            return (TRepository)_repositories[name];

        if (_repositories.TryGetValue(name, out repository)) return (TRepository) repository;

        throw new InvalidOperationException(
            $"Repository of type {name} couldn't be added to and/or retrieved.");
    }

    /// <inheritdoc />
    public async Task RollbackAsync()
    {
        await _transaction.AbortAsync();
    }

    /// <inheritdoc />
    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
        _transaction?.Dispose();
        _transaction = DB.Transaction();
    }

    /// <inheritdoc />
    public async Task CommitAsync(string userId)
    {
        _transaction.ModifiedBy = new AuditEntry(userId);
        await _transaction.CommitAsync();
        _transaction?.Dispose();
        _transaction = DB.Transaction();
    }

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
            _transaction?.Dispose();
        }

        _repositories = null;

        _disposed = true;
    }
}