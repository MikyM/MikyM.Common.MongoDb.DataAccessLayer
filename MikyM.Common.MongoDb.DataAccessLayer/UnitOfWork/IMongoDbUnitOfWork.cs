using MikyM.Common.DataAccessLayer;
using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;

/// <summary>
/// Unit of work definition
/// </summary>
public interface IMongoDbUnitOfWork : IUnitOfWorkBase
{
    /// <summary>
    /// Inner <see cref="Transaction"/>
    /// </summary>
    Transaction Transaction { get; }
    /// <summary>
    /// Name of the database for which this unit of work was created
    /// </summary>
    public string Database { get; }
}