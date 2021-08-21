using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.WebCore.LogExtend
{
    /// <summary>
    /// 扩展个方法，让ILoggerFactory去AddProvider
    /// </summary>
    public static class CustomConsoleLoggerExtensions
    {
        //直接传递对象
        public static ILoggerFactory AddCustomConsoleLogger(this ILoggerFactory loggerFactory, CustomConsoleLoggerConfiguration config)
        {
            loggerFactory.AddProvider(new CustomConsoleLoggerProvider(config));
            return loggerFactory;
        }

        /// <summary>
        /// 默认对象
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        public static ILoggerFactory AddCustomConsoleLogger(this ILoggerFactory loggerFactory)
        {
            var config = new CustomConsoleLoggerConfiguration();
            return loggerFactory.AddCustomConsoleLogger(config);
        }

        /// <summary>
        /// 传递Action
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ILoggerFactory AddCustomConsoleLogger(this ILoggerFactory loggerFactory, Action<CustomConsoleLoggerConfiguration> configure)
        {
            var config = new CustomConsoleLoggerConfiguration();
            Console.WriteLine("**************");

            configure.Invoke(config);

            Console.WriteLine("**************");
            return loggerFactory.AddCustomConsoleLogger(config);
        }
    }
}
