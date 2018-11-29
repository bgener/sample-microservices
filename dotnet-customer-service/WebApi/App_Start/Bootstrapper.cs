using Autofac;
using Autofac.Features.Variance;
using Autofac.Integration.WebApi;
using Data;
using Data.Repositories;
using Domain.Interfaces;
using WebApi.Infrastructure;

namespace WebApi
{
    public static class Bootstrapper
    {
        public static IContainer Initialize()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new ContravariantRegistrationSource());

            //Persistence
            builder.RegisterType<DatabaseInitializer>();
            builder.RegisterType<InMemoryDatabase>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PersonRepository>().AsImplementedInterfaces();
            builder.RegisterType<GroupRepository>().AsImplementedInterfaces();

            //Web API
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly).InstancePerRequest();

            //Domain
            builder.RegisterType<QueryExecutor>().AsImplementedInterfaces();
            builder.RegisterType<CommandProcessor>().AsImplementedInterfaces();
            builder.RegisterType<InMemoryEventBus>().AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(ICommand).Assembly).AsImplementedInterfaces();
            
            return builder.Build();
        }
    }
}
