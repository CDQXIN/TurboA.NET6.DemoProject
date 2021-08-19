using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.AgileFramework.WebCore.LogExtend
{
    /// <summary>
    /// 写日志具体操作
    /// </summary>
    public class CustomConsoleLogger : ILogger
    {
        private readonly string _loggerName;
        private readonly CustomConsoleLoggerConfiguration _CustomColorConsoleLoggerConfiguration;

        public CustomConsoleLogger(string loggerName, CustomConsoleLoggerConfiguration config)
        {
            this._loggerName = loggerName;
            this._CustomColorConsoleLoggerConfiguration = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// 是否写入日志
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
            //return logLevel == _CustomColorConsoleLoggerConfiguration.LogLevel;
            //return logLevel >= this._CustomColorConsoleLoggerConfiguration.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (this._CustomColorConsoleLoggerConfiguration.EventId == 0 || this._CustomColorConsoleLoggerConfiguration.EventId == eventId.Id)
            {
                //日志输出到控制台
                Console.WriteLine($"Eleven Custom Log: {logLevel} - {eventId.Id} " + $"- {this._loggerName} - {formatter(state, exception)}");
            }
        }
    }
}
