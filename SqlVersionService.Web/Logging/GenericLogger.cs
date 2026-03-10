using Microsoft.Extensions.Logging;
using System;

namespace SqlVersionService.Web.Logging
{
    public sealed class GenericLogger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public GenericLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(T).FullName);
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        void ILogger.Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            System.Exception exception,
            System.Func<TState, System.Exception, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}