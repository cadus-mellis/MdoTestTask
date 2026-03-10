using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using SqlVersionService.Application.Abstractions;
using SqlVersionService.Infrastructure.Models;
using SqlVersionService.Application.Services;
using SqlVersionService.Infrastructure.Abstractions;
using SqlVersionService.Infrastructure.Services;
using SqlVersionService.Web.Background;
using SqlVersionService.Web.Filters;
using SqlVersionService.Web.Logging;

namespace SqlVersionService.Web.CompositionRoot
{
    public static class AutofacConfig
    {
        public static IContainer Configure(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            var sessionOptions = new SqlSessionOptions
            {
                SessionTimeoutSeconds = 300,
                CleanupIntervalSeconds = 60
            };

            builder.RegisterInstance(sessionOptions).SingleInstance();
            
            ConfigureLogger(builder);

            builder.RegisterType<InMemorySqlConnectionRegistry>()
                .As<ISqlConnectionRegistry>()
                .SingleInstance();

            builder.RegisterType<SqlConnectionSessionService>()
                .As<ISqlConnectionSessionService>()
                .SingleInstance();

            builder.RegisterType<SessionCleanupJob>()
                .SingleInstance();

            builder.RegisterType<UnhandledExceptionFilter>()
                .SingleInstance();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }

        private static void ConfigureLogger(ContainerBuilder builder)
        {
            // Serilog root logger
            builder.RegisterInstance(Log.Logger)
                .As<Serilog.ILogger>()
                .SingleInstance();
            // Microsoft logging factory
            builder.Register(c =>
            {
                var factory = new LoggerFactory();
                factory.AddSerilog(Log.Logger, dispose: false);
                return factory;
            })
                .As<ILoggerFactory>()
                .SingleInstance();
            // ILogger<T>
            builder.RegisterGeneric(typeof(GenericLogger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
        }
    }
}