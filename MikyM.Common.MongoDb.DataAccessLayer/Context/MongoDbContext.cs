using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer.Context;

/// <summary>
/// Database context for MongoDb
/// </summary>
public abstract class MongoDbContext : IDisposable
{
    /// <summary>
    /// Name of the database
    /// </summary>
    public string DatabaseName { get; }
    /// <summary>
    /// Inner <see cref="Transaction"/>
    /// </summary>
    public Transaction Transaction { get; private set; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="databaseName"></param>
    public MongoDbContext(string databaseName)
    {
        DatabaseName = databaseName;
        Transaction = DB.Transaction(databaseName);
    }
    
    /// <summary>
    /// Rolls the current pending changes back
    /// </summary>
    public async Task RollbackAsync()
    {
        await Transaction.AbortAsync();
    }
    
    /// <summary>
    /// Commits pending changes
    /// </summary>
    public async Task CommitAsync()
    {
        await Transaction.CommitAsync();
        Transaction?.Dispose();
        Transaction = DB.Transaction();
    }
    
    /// <summary>
    /// Commits pending changes
    /// </summary>
    /// <param name="userId">Id of user responsible for the changes</param>
    public async Task CommitAsync(string userId)
    {
        Transaction.ModifiedBy = new AuditEntry(userId);
        await Transaction.CommitAsync();
        Transaction?.Dispose();
        Transaction = DB.Transaction();
    }

    /// <summary>
    /// Disposes current context and the underlying transaction
    /// </summary>
    public void Dispose()
    {
        Transaction.Dispose();
    }
}