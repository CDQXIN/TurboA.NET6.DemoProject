using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.AgileFramework.Pandora.CustomContainer
{
    /// <summary>
    /// 这个是常量
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class ZhaoxiParameterShortNameAttribute : Attribute
    {
        public string ShortName { get; private set; }
        public ZhaoxiParameterShortNameAttribute(string shortName)
        {
            this.ShortName = shortName;
        }
    }
}
