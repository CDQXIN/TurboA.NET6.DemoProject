using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace TurboA.AgileFramework.Pandora.CustomAOP
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomAOPTest
    {
        public static void Show()
        {
            ProxyGenerator generator = new ProxyGenerator();
            CustomInterceptor interceptor = new CustomInterceptor();//自定义拦截器
            CommonClass testClass = generator.CreateClassProxy<CommonClass>(interceptor);

            //Castle通过Emit技术去动态生成了一个代理类，这个类是目标类的子类CommonClass
            Console.WriteLine("当前类型:{0},父类型:{1}", testClass.GetType(), testClass.GetType().BaseType);
            Console.WriteLine();
            testClass.MethodInterceptor();

            Console.WriteLine();
            testClass.MethodNoInterceptor();
            Console.WriteLine();
            //Console.ReadLine();
        }
    }
}
