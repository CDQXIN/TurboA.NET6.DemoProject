using System;
using Zhaoxi.NET6.TricksCommon;
using Zhaoxi.NET6.TricksInterface;

namespace Zhaoxi.NET6.TricksService
{
    public class LogService : ILogService
    {
        public void WirteLogDB(string message)
        {
            Console.WriteLine($"DB Write {message}");
        }
    }
}
