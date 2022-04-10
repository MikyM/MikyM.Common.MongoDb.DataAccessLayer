
namespace MikyM.Common.MongoDb.DataAccessLayer.Exceptions;

/// <summary>
/// 
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    public NotFoundException()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public NotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public NotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
