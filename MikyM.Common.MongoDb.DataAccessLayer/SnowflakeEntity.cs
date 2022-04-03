using MikyM.Common.Domain;
using MongoDB.Entities;
// ReSharper disable InconsistentNaming

namespace MikyM.Common.MongoDb.DataAccessLayer;

/// <summary>
/// 
/// </summary>
public class SnowflakeEntity : Entity
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string GenerateNewID()
        => IdGeneratorFactory.Build().CreateId().ToString();

    /// <summary>
    /// 
    /// </summary>
    public virtual DateTime? CreatedAt { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public virtual DateTime? UpdatedAt { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public virtual bool IsDisabled { get; set; }

    /// <summary>
    /// 
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
        if (obj is not SnowflakeEntity other)
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
    public static bool operator ==(SnowflakeEntity? a, SnowflakeEntity? b)
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
    public static bool operator !=(SnowflakeEntity a, SnowflakeEntity b)
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