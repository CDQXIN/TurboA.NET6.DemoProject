using System;

namespace Zhaoxi.NET6.TricksCommon
{
    public class LogHelper
    {
        public static void LogMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff")}");
            //如果我想写入数据库呢？--引用Zhaoxi.NET6.TricksService？
        }
        #region MyRegion
        /// <summary>
        /// 委托看上去是个方法--但其实还包括所在实例
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logInDBAction"></param>
        public static void LogMessage(string message, Action<string> logInDBAction)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff")}");
            logInDBAction(message);
        }
        #endregion

    }
}
