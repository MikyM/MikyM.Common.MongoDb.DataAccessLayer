namespace MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;

/// <summary>
/// Factory for <see cref="IMongoDbUnitOfWork"/>
/// </summary>
public interface IMongoDbUnitOfWorkFactory
{
    /// <summary>
    /// Gets a <see cref="IMongoDbUnitOfWork"/> for the default database
    /// </summary>
    /// <returns><see cref="IMongoDbUnitOfWork"/> for the default database</returns>
    IMongoDbUnitOfWork GetUnitOfWork();
    /// <summary>
    /// Gets a <see cref="IMongoDbUnitOfWork"/> for given database name
    /// </summary>
    /// <param name="database">Name of the database to use</param>
    /// <returns><see cref="IMongoDbUnitOfWork"/> for given database name</returns>
    IMongoDbUnitOfWork GetUnitOfWork(string database);
}