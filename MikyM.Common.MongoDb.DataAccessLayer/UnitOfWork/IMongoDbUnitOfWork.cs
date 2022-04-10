using MikyM.Common.MongoDb.DataAccessLayer.Repositories;
using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;

/// <summary>
/// Unit of work definition
/// </summary>
public interface IMongoDbUnitOfWork : IDisposable
{
    /// <summary>
    /// Inner <see cref="Transaction"/>
    /// </summary>
    Transaction Transaction { get; }
    /// <summary>
    /// Name of the database for which this unit of work was created
    /// </summary>
    public string Database { get; }
    
    /// <summary>
    /// Gets a repository of a given type
    /// </summary>
    /// <typeparam name="TRepository">Type of the repository to get</typeparam>
    /// <returns>Wanted repository</returns>
    TRepository GetRepository<TRepository>() where TRepository : class, IBaseRepository;
    /// <summary>
    /// Commits changes
    /// </summary>
    /// <returns>Number of affected rows</returns>
    Task CommitAsync();
    /// <summary>
    /// Commits changes
    /// </summary>
    /// <param name="userId">Id of the user that is responsible for doing changes</param>
    /// <returns>Number of affected rows</returns>
    Task CommitAsync(string userId);
    /// <summary>
    /// Rolls the transaction back
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    Task RollbackAsync();
}