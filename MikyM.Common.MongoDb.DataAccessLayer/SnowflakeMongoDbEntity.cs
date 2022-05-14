using MikyM.Common.Domain;
using MongoDB.Entities;
// ReSharper disable InconsistentNaming

namespace MikyM.Common.MongoDb.DataAccessLayer;

/// <summary>
/// Snowflake entity for MongoDb
/// </summary>
public class SnowflakeMongoDbEntity : Entity
{
    /// <inheritdoc/>
    public override string GenerateNewID()
        => IdGeneratorFactory.Build().CreateId().ToString();

    /// <summary>
    /// Created at date
    /// </summary>
    public virtual DateTime? CreatedAt { get; set; }
    /// <summary>
    /// Updated at date
    /// </summary>
    public virtual DateTime? UpdatedAt { get; set; }
    /// <summary>
    /// Whether entity is disabled
    /// </summary>
    public virtual bool IsDisabled { get; set; }

    /// <summary>
    /// Returns the Id of this entity
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => ID;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not SnowflakeMongoDbEntity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetUnproxiedType(this) != GetUnproxiedType(other))
            return false;

        if (ID.Equals(default) || other.ID.Equals(default))
            return false;

        return ID.Equals(other.ID);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator ==(SnowflakeMongoDbEntity? a, SnowflakeMongoDbEntity? b)
    {
        if (a  is null && b  is null)
            return true;

        if (a  is null || b  is null)
            return false;

        return a.Equals(b);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator !=(SnowflakeMongoDbEntity a, SnowflakeMongoDbEntity b)
    {
        return !(a == b);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return (GetUnproxiedType(this) + ID).GetHashCode();
    }

    private static Type GetUnproxiedType(object obj)
    {
        const string EFCoreProxyPrefix = "Castle.Proxies.";
        const string NHibernateProxyPostfix = "Proxy";

        Type type = obj.GetType();
        string typeString = type.ToString();

        if (typeString.Contains(EFCoreProxyPrefix) || typeString.EndsWith(NHibernateProxyPostfix))
            return type.BaseType!;

        return type;
    }
}