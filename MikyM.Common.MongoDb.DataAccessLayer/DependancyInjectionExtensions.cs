using Autofac;
using IdGen;
using Microsoft.Extensions.Options;
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
    /// <param name="builder">Current instance of <see cref="ContainerBuilder"/></param>
    /// <param name="options"><see cref="Action"/> that configures DAL.</param>
    public static void AddMongoDbDataAccessLayer(this ContainerBuilder builder, Action<MongoDbDataAccessConfiguration>? options = null)
    {
        var config = new MongoDbDataAccessConfiguration(builder);
        options?.Invoke(config);

        builder.Register(x => config).As<IOptions<MongoDbDataAccessConfiguration>>().SingleInstance();

        if (config.IdGeneratorOptions is not null)
        {
            var sub = builder.Register(_ => new IdGenerator(0, config.IdGeneratorOptions))
                .AsSelf()
                .SingleInstance();
        }

        builder.RegisterType<MongoDbUnitOfWorkFactory>().As<IMongoDbUnitOfWorkFactory>().InstancePerLifetimeScope();
        builder.RegisterType<MongoDbUnitOfWork>().As<IMongoDbUnitOfWork>().UsingConstructor(typeof(IOptions<MongoDbDataAccessConfiguration>))
            .InstancePerLifetimeScope();
        
        if (config.Databases is not null && config.Databases.Length > 0)
            foreach (var database in config.Databases)
                builder.RegisterType<MongoDbUnitOfWork>().Named<IMongoDbUnitOfWork>(database)
                    .UsingConstructor(typeof(string), typeof(IOptions<MongoDbDataAccessConfiguration>)).WithParameter("database", database).InstancePerLifetimeScope();
    }
}