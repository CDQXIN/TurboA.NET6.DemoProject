using System;
using TurboA.NET6.TricksCommon;
using TurboA.NET6.TricksInterface;

namespace TurboA.NET6.TricksService
{
    public class LogService : ILogService
    {
        public void WirteLogDB(string message)
        {
            Console.WriteLine($"DB Write {message}");
        }
    }
}
