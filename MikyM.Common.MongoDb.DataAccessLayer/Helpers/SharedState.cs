namespace MikyM.Common.MongoDb.DataAccessLayer.Helpers;

internal static class SharedState
{
    internal static bool DisableOnBeforeSaveChanges { get; set; } = false;
}