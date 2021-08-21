using System;
using TurboA.NET6.TricksCommon;
using TurboA.NET6.TricksInterface;

namespace TurboA.NET6.TricksService
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
