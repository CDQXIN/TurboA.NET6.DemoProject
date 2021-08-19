using System;
using Zhaoxi.NET6.TricksCommon;
using Zhaoxi.NET6.TricksInterface;

namespace Zhaoxi.NET6.TricksService
{
    public class UserService : IUserService
    {
        public void Login(string account, string password)
        {
            Console.WriteLine($"{account} Login Success");
            LogHelper.LogMessage($"{account} Login Success");

            LogHelper.LogMessage($"{account} Login Success", s =>
            {
                new LogService().WirteLogDB(s);
            });
        }
    }
}
