using System;
using System.Collections.Generic;
using System.Text;

namespace TurboA.AgileFramework.Pandora.CustomAOP
{
    /// <summary>
    /// 普通类
    /// </summary>
    public class CommonClass
    {
        public virtual void MethodInterceptor()
        {
            Console.WriteLine("This's Interceptor");
        }

        public void MethodNoInterceptor()
        {
            Console.WriteLine("This's without Interceptor");
        }
    }

    
}
