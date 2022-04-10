using Autofac;
using IdGen;
using Microsoft.Extensions.Options;
using MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;

namespace MikyM.Common.MongoDb.DataAccessLayer;

/// <summary>
/// Configuration for Data Access Layer
/// </summary>
public class MongoDbDataAccessConfiguration : IOptions<MongoDbDataAccessConfiguration>
{
    /// <summary>
    /// Options for the data access layer
    /// </summary>
    /// <param name="builder"></param>
    public MongoDbDataAccessConfiguration(ContainerBuilder builder)
    {
        _builder = builder;
    }

    private readonly ContainerBuilder _builder;
    private Func<IMongoDbUnitOfWork, Task>? _onBeforeSaveChanges;
    private string[]? _databases;

    /// <summary>
    /// Whether to cache include expressions (queries are evaluated faster).
    /// </summary>
    public bool EnableIncludeCache { get; set; } = false;

    /// <summary>
    /// Action to execute before each <see cref="IMongoDbUnitOfWork.CommitAsync()"/>
    /// </summary>
    public Func<IMongoDbUnitOfWork, Task>? OnBeforeSaveChanges
    {
        get => _onBeforeSaveChanges;
        set
        {
            _onBeforeSaveChanges = value;
        }
    }
    
    /// <summary>
    /// List of database names if using more than one MongoDb (list also the default one) to register named <see cref="IMongoDbUnitOfWork"/> for
    /// </summary>
    public string[]? Databases
    {
        get => _databases;
        set
        {
            _databases = value;
        }
    }

    /// <summary>
    /// Gets or sets settings for <see cref="IdGenerator"/>
    /// </summary>
    public IdGeneratorOptions? IdGeneratorOptions { get; set; }
    /// <summary>
    /// Gets or sets the Id for <see cref="IdGenerator"/>
    /// </summary>
    public int IdGeneratorId { get; set; }

    /// <summary>
    /// Instance of options
    /// </summary>
    public MongoDbDataAccessConfiguration Value => this;
}