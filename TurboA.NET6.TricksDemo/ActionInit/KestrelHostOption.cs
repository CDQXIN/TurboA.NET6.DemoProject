using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.NET6.TricksDemo.ActionInit
{
    /// <summary>
    /// 提供默认值
    /// </summary>
    public class KestrelHostOption
    {
        /// <summary>
        /// 默认是fasle
        /// </summary>
        public bool EnableAltSvc { get; set; } = false;
        public bool DisableStringReuse { get; set; }
        public bool AllowSynchronousIO { get; set; }
        public bool AllowResponseHeaderCompression { get; set; }
        public bool AddServerHeader { get; set; }
        public KestrelServerLimits Limits { get; }

        public void Init1()
        {
            Console.WriteLine("初始化1");
        }
        public void Init2()
        {
            Console.WriteLine("初始化2");
        }
        public void Init3()
        {
            Console.WriteLine("初始化3");
        }
    }
}
