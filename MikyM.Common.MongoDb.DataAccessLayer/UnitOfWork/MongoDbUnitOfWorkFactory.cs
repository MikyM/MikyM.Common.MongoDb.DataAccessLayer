using Autofac;

namespace MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;

/// <summary>
/// Factory for <see cref="IMongoDbUnitOfWork"/>
/// </summary>
public class MongoDbUnitOfWorkFactory : IMongoDbUnitOfWorkFactory
{
    /// <summary>
    /// Inner scope
    /// </summary>
    private readonly ILifetimeScope _lifetimeScope;
    
    /// <summary>
    /// Initializes a new instance of <see cref="MongoDbUnitOfWorkFactory"/>
    /// </summary>
    /// <param name="lifetimeScope">Lifetime scope for this instance</param>
    public MongoDbUnitOfWorkFactory(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    /// <inheritdoc/>
    public IMongoDbUnitOfWork GetUnitOfWork()
        => _lifetimeScope.Resolve<IMongoDbUnitOfWork>();

    /// <inheritdoc/>
    public IMongoDbUnitOfWork GetUnitOfWork(string database)
        => _lifetimeScope.ResolveNamed<IMongoDbUnitOfWork>(database);
}