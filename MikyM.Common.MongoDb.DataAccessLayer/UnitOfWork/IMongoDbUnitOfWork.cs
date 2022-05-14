using MikyM.Common.DataAccessLayer;
using MikyM.Common.MongoDb.DataAccessLayer.Context;

namespace MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;


/// <summary>
/// Unit of work definition
/// </summary>
public interface IMongoDbUnitOfWork : IUnitOfWorkBase
{
}

/// <summary>
/// Unit of work definition
/// </summary>
public interface IMongoDbUnitOfWork<TContext> : IMongoDbUnitOfWork where TContext : MongoDbContext
{
    /// <summary>
    /// Current <see cref="MongoDbContext"/>
    /// </summary>
    TContext Context { get; }
}