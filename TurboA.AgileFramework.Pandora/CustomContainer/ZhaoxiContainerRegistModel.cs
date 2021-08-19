using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.AgileFramework.Pandora.CustomContainer
{
    public class ZhaoxiContainerRegistModel
    {
        public Type TargetType { get; set; }

        public LifetimeType Lifetime { get; set; }
        /// <summary>
        /// 仅限单例
        /// </summary>
        public object SingletonInstance { get; set; }
    }

    public enum LifetimeType
    {
        Transient,
        Singleton,
        Scope,
        PerThread//线程单例
        //外部可释放单例
    }
}
