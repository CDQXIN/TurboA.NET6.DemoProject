using System;
using TurboA.AgileFramework.Pandora.CustomAOP;

namespace TurboA.NET6.TricksDemo.CustomeAOP
{
    public interface ITricksServiceA
    {
        [Login]
        [Monitor]
        [LogBefore]
        [LogAfter]
        void Show();

        void Show1();
    }
}
