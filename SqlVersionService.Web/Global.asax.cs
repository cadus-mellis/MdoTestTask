using Autofac;
using Serilog.Debugging;
using SqlVersionService.Application.Abstractions;
using SqlVersionService.Web.App_Start;
using SqlVersionService.Web.Background;
using SqlVersionService.Web.CompositionRoot;
using SqlVersionService.Web.Logging;
using System;
using System.IO;
using System.Web.Http;

namespace SqlVersionService.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IContainer _container;
        private SessionCleanupJob _cleanupJob;

        protected void Application_Start()
        {
            SerilogConfig.Configure();
            var selfLogFile = File.CreateText(@"C:\git\tests\temp\serilog-selflog.txt");
            SelfLog.Enable(TextWriter.Synchronized(selfLogFile));

            var config = GlobalConfiguration.Configuration;

            config.MapHttpAttributeRoutes();

            SwaggerConfig.Register(config);

            _container = AutofacConfig.Configure(config);

            config.Filters.Add(_container.Resolve<Filters.UnhandledExceptionFilter>());

            config.EnsureInitialized();

            _container.Resolve<ISqlConnectionSessionService>();

            _cleanupJob = _container.Resolve<SessionCleanupJob>();
            _cleanupJob.Start();
        }

        protected void Application_End()
        {
            _container?.ResolveOptional<ISqlConnectionSessionService>()?.CloseAll();
            _cleanupJob?.Dispose();
            Serilog.Log.CloseAndFlush();
            _container?.Dispose();
        }
    }
}