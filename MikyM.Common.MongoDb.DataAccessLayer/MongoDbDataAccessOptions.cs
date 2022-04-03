using Autofac;
using IdGen;
using MikyM.Common.MongoDb.DataAccessLayer.Helpers;

namespace MikyM.Common.MongoDb.DataAccessLayer;

/// <summary>
/// Options for Data Access Layer.
/// </summary>
public class MongoDbDataAccessOptions
{
    public MongoDbDataAccessOptions(ContainerBuilder builder)
    {
        _builder = builder;
    }

    private readonly ContainerBuilder _builder;
    private bool _disableOnBeforeSaveChanges = false;

    /// <summary>
    /// Whether to cache include expressions (queries are evaluated faster).
    /// </summary>
    public bool EnableIncludeCache { get; set; } = false;

    /// <summary>
    /// Disables the insertion of audit log entries
    /// </summary>
    public bool DisableOnBeforeSaveChanges
    {
        get => _disableOnBeforeSaveChanges;
        set
        {
            _disableOnBeforeSaveChanges = value;
            SharedState.DisableOnBeforeSaveChanges = value;
        }
    }

    /// <summary>
    /// Gets or set settings for <see cref="IdGenerator"/>
    /// </summary>
    public IdGeneratorOptions? IdGeneratorOptions { get; set; }
}