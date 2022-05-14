using System.Reflection;
using Autofac;
using Microsoft.Extensions.Options;
using MikyM.Common.DataAccessLayer;
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

        builder.RegisterType<MongoDbUnitOfWorkFactory>().As<IMongoDbUnitOfWorkFactory>().InstancePerLifetimeScope();
        builder.RegisterType<MongoDbUnitOfWork>().As<IMongoDbUnitOfWork>().UsingConstructor(typeof(IOptions<MongoDbDataAccessConfiguration>))
            .InstancePerLifetimeScope();

        if (config.Databases is null || config.Databases.Count <= 0) 
            return;
        
        foreach (var database in config.Databases)
            builder.RegisterType<MongoDbUnitOfWork>().Named<IMongoDbUnitOfWork>(database)
                .UsingConstructor(typeof(string), typeof(IOptions<MongoDbDataAccessConfiguration>)).WithParameter("database", database).InstancePerLifetimeScope();
    }
}