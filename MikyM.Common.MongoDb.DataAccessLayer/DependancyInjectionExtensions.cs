using Autofac;
using IdGen;
using MikyM.Autofac.Extensions;
using MikyM.Common.Domain;
using MikyM.Common.MongoDb.DataAccessLayer.Repositories;
using MikyM.Common.MongoDb.DataAccessLayer.UnitOfWork;
using MongoDB.Entities;

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
    public static void AddMongoDbDataAccessLayer(this ContainerBuilder builder, Action<MongoDbDataAccessOptions>? options = null)
    {
        var config = new MongoDbDataAccessOptions(builder);
        options?.Invoke(config);

        if (config.IdGeneratorOptions is not null)
        {
            var sub = builder.Register(_ => new IdGenerator(0, config.IdGeneratorOptions))
                .AsSelf()
                .SingleInstance();
        }

        var ctorFinder = new AllConstructorsFinder();

        builder.RegisterGeneric(typeof(ReadOnlyMongoDbRepository<>))
            .As(typeof(IReadOnlyMongoDbRepository<>))
            .FindConstructorsWith(ctorFinder)
            .InstancePerLifetimeScope();
        builder.RegisterGeneric(typeof(MongoDbRepository<>))
            .As(typeof(IMongoDbRepository<>))
            .FindConstructorsWith(ctorFinder)
            .InstancePerLifetimeScope();
        builder.RegisterType<MongoDbUnitOfWork>().As<IMongoDbUnitOfWork>().InstancePerLifetimeScope();
        builder.RegisterType<Transaction>().AsSelf().InstancePerLifetimeScope();
    }
}