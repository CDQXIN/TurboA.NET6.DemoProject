using System;
using Zhaoxi.AgileFramework.Pandora.CustomAOP;

namespace Zhaoxi.NET6.TricksDemo.CustomeAOP
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
