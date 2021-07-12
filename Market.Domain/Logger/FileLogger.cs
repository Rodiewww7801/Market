using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Market.Domain.Logger
{
    public class FileLogger : ILogger
    {
        private string _path;
        private static object _lock = new object();

        public FileLogger(string path)
        {
            _path = path;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        string GetShortLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return "TRCE";
                case LogLevel.Debug:
                    return "DBUG";
                case LogLevel.Information:
                    return "INFO";
                case LogLevel.Warning:
                    return "WARN";
                case LogLevel.Error:
                    return "FAIL";
                case LogLevel.Critical:
                    return "CRIT";
            }
            return logLevel.ToString().ToUpper();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

            string message = null;
            if (formatter != null)
            {
                message = formatter(state, exception);
            }


            var logBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(message))
            {
                logBuilder.Append(DateTime.Now.ToString("o"));
                logBuilder.Append('\t');
                logBuilder.Append(GetShortLogLevel(logLevel));
                logBuilder.Append("\t[");
                logBuilder.Append("]");
                logBuilder.Append("\t[");
                logBuilder.Append(eventId);
                logBuilder.Append("]\t");
                logBuilder.Append(message);
            }

            if (exception != null)
            {
                logBuilder.AppendLine(exception.ToString());
            }
            lock (_lock)
            {
                File.AppendAllText(_path, logBuilder.ToString() +Environment.NewLine);
            }

        }
    }
}
