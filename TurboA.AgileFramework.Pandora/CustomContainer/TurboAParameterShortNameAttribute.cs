using System;
using System.Collections.Generic;
using System.Text;

namespace TurboA.AgileFramework.Pandora.CustomContainer
{
    /// <summary>
    /// 这个是常量
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class TurboAParameterShortNameAttribute : Attribute
    {
        public string ShortName { get; private set; }
        public TurboAParameterShortNameAttribute(string shortName)
        {
            this.ShortName = shortName;
        }
    }
}
