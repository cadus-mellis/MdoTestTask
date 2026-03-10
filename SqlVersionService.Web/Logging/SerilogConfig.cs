using System;
using System.IO;
using Serilog;

namespace SqlVersionService.Web.Logging
{
    public static class SerilogConfig
    {
        public static void Configure()
        {
            var logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logsDirectory);

            var logFilePath = Path.Combine(logsDirectory, "service-.log");

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Serilog initialized");
        }
    }
}