using System.Collections.Generic;
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
    private Dictionary<string, Func<IMongoDbUnitOfWork, Task>>? _onBeforeSaveChangesActions;
    private string[]? _databases;

    /// <summary>
    /// Whether to cache include expressions (queries are evaluated faster).
    /// </summary>
    public bool EnableIncludeCache { get; set; } = false;

    /// <summary>
    /// Action to execute before each <see cref="IMongoDbUnitOfWork.CommitAsync()"/>
    /// </summary>
    public Dictionary<string, Func<IMongoDbUnitOfWork, Task>>? OnBeforeSaveChangesActions
        => _onBeforeSaveChangesActions;
    
    /// <summary>
    /// Adds an on before save changes action for a given database
    /// </summary>
    /// <param name="action">Action to perform</param>
    /// <param name="database">Name of the database for the action</param>
    /// <exception cref="NotSupportedException">Throw when trying to register second action for same context type</exception>
    public void AddOnBeforeSaveChangesAction(string database, Func<IMongoDbUnitOfWork, Task> action)
    {
        _onBeforeSaveChangesActions ??= new Dictionary<string, Func<IMongoDbUnitOfWork, Task>>();
        if (_onBeforeSaveChangesActions.TryGetValue(database, out _))
            throw new NotSupportedException("Multiple actions for same context aren't supported");
        _onBeforeSaveChangesActions.Add(database, action);
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