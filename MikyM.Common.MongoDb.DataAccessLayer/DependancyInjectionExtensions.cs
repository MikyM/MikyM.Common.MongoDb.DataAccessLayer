using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdGen;
using Microsoft.Extensions.Options;
using MikyM.Common.Domain;
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

        builder.RegisterType<MongoDbUnitOfWorkFactory>().As<IMongoDbUnitOfWorkFactory>().InstancePerLifetimeScope();
        builder.RegisterType<MongoDbUnitOfWork>().As<IMongoDbUnitOfWork>().UsingConstructor(typeof(IOptions<MongoDbDataAccessConfiguration>))
            .InstancePerLifetimeScope();

        if (config.Databases is null || config.Databases.Count <= 0) 
            return;
        
        foreach (var database in config.Databases)
            builder.RegisterType<MongoDbUnitOfWork>().Named<IMongoDbUnitOfWork>(database)
                .UsingConstructor(typeof(string), typeof(IOptions<MongoDbDataAccessConfiguration>)).WithParameter("database", database).InstancePerLifetimeScope();
    }
    
    /// <summary>
    /// Sets factory method for <see cref="IdGeneratorFactory"/>
    /// </summary>
    /// <param name="provider">Current instance of <see cref="IServiceProvider"/></param>
    /// <returns>Current instance of <see cref="IServiceProvider"/></returns>
    public static IServiceProvider ConfigureIdGeneratorFactory(this IServiceProvider provider)
    {
        IdGeneratorFactory.SetFactory(() => provider.GetAutofacRoot().Resolve<IdGenerator>());

        return provider;
    }
    
    /// <summary>
    /// Registers <see cref="IdGenerator"/> with the container
    /// </summary>
    /// <param name="dataAccessOptions"></param>
    /// <param name="options">Id generator configuration</param>
    /// <returns>Current <see cref="MongoDbDataAccessConfiguration"/> instance</returns>
    public static MongoDbDataAccessConfiguration AddSnowflakeIdGenerator(this MongoDbDataAccessConfiguration dataAccessOptions, Action<IdGeneratorConfiguration> options)
    {
        var opt = new IdGeneratorConfiguration();
        options(opt);
        opt.Validate();

        dataAccessOptions.Builder.Register(_ => new IdGenerator(opt.GeneratorId,
                new IdGeneratorOptions(opt.IdStructure, opt.DefaultTimeSource, opt.SequenceOverflowStrategy)))
            .AsSelf()
            .SingleInstance();

        return dataAccessOptions;
    }
}