using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Options;
using MikyM.Common.DataAccessLayer;
using MikyM.Common.MongoDb.DataAccessLayer.Context;
using MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;

namespace MikyM.Common.MongoDb.DataAccessLayer;

/// <summary>
/// DI extensions for <see cref="ContainerBuilder"/>
/// </summary>
public static class DependancyInjectionExtensions
{
    /// <summary>
    /// Adds Data Access Layer to the application.
    /// </summary>
    /// <param name="configuration">Current instance of <see cref="DataAccessConfiguration"/></param>
    /// <param name="options"><see cref="Action"/> that configures DAL.</param>
    public static void AddMongoDbLayer(this DataAccessConfiguration configuration,
        Action<MongoDbDataAccessConfiguration>? options = null)
    {
        if (configuration.GetType().GetField("Builder", BindingFlags.Instance |
                                                           BindingFlags.NonPublic |
                                                           BindingFlags.Public)
                ?.GetValue(configuration) is not ContainerBuilder builder)
            throw new InvalidOperationException();

        var config = new MongoDbDataAccessConfiguration(builder);
        options?.Invoke(config);

        builder.Register(x => config).As<IOptions<MongoDbDataAccessConfiguration>>().SingleInstance();
        
        builder.RegisterGeneric(typeof(MongoDbUnitOfWork<>)).As(typeof(IMongoDbUnitOfWork<>)).InstancePerLifetimeScope();
    }

    /// <summary>
    /// Adds database context to the data access layer
    /// </summary>
    /// <param name="configuration">Current config</param>
    /// <param name="databaseName">Name of the database</param>
    /// <remarks>Bear in mind that TypedParameter (with string type) is used to provide database name to the base context, thus your ctor can't have other string params</remarks>
    /// <returns>Current config</returns>
    public static MongoDbDataAccessConfiguration AddMongoDbContext<TContext>(this MongoDbDataAccessConfiguration configuration,
        string databaseName) where TContext : MongoDbContext
    {
        configuration.Builder.RegisterType(typeof(TContext)).AsSelf()
            .WithParameter(new TypedParameter(typeof(string), databaseName)).InstancePerLifetimeScope();

        return configuration;
    }

    /// <summary>
    /// Adds database context to the data access layer
    /// </summary>
    /// <param name="configuration">Current config</param>
    /// <param name="databaseName">Name of the database</param>
    /// <param name="parameters">Parameters to pass to the constructor</param>
    /// <returns>Current config</returns>
    public static MongoDbDataAccessConfiguration AddMongoDbContext<TContext>(this MongoDbDataAccessConfiguration configuration,
        string databaseName, IEnumerable<Parameter> parameters) where TContext : MongoDbContext
    {
        configuration.Builder.RegisterType(typeof(TContext)).AsSelf()
            .WithParameters(parameters).InstancePerLifetimeScope();

        return configuration;
    }
}