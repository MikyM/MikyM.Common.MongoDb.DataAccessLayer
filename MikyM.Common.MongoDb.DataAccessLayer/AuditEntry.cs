using MongoDB.Entities;

namespace MikyM.Common.MongoDb.DataAccessLayer;

/// <summary>
/// Audit database entry
/// </summary>
public class AuditEntry : ModifiedBy
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    public AuditEntry(string userId)
    {
        UserID = userId;
    }
}