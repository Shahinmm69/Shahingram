using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Data;
using Data.Contract;
using Data.Repositories;
using Entities.Common;
using Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebFramework.Configuration
{
    public static class AutofacConfigurationExtensions
    {
        public static void AddServices(this ContainerBuilder containerBuilder)
        {
            //RegisterType > As > Liftetime
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            containerBuilder.RegisterGeneric(typeof(DeletionRepository<>)).As(typeof(IDeletionRepository<>)).InstancePerLifetimeScope();
            containerBuilder.RegisterGeneric(typeof(CreationRepository<>)).As(typeof(ICreationRepository<>)).InstancePerLifetimeScope();
            containerBuilder.RegisterGeneric(typeof(ModificationRepository<>)).As(typeof(IModificationRepository<>)).InstancePerLifetimeScope();

            var commonAssembly = typeof(SiteSettings).Assembly;
            var entitiesAssembly = typeof(IEntity).Assembly;
            var dataAssembly = typeof(ApplicationDbContext).Assembly;
            var servicesAssembly = typeof(JwtService).Assembly;

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<IScopedDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ISingletonDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        public static IServiceProvider BuildAutofacServiceProvider(this IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            //Register Services to Autofac ContainerBuilder
            containerBuilder.AddServices();

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}
