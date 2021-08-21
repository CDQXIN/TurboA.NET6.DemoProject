using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.NET6.TricksDemo.ActionInit
{
    /// <summary>
    /// 静态扩展方法
    /// </summary>
    public class KestrelHostExtend
    {
        public static KestrelHost BuildAndConfigureKestrelHost(Action<KestrelHostOption> action)
        {
            KestrelHost kestrelHost = new KestrelHost();
            //kestrelHost.KestrelHostOption = new KestrelHostOption();

            action.Invoke(kestrelHost.KestrelHostOption);

            return kestrelHost;
        }
    }
}
