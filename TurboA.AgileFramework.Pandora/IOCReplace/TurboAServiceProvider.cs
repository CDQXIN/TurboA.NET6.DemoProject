using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TurboA.AgileFramework.Pandora.CustomContainer;

namespace TurboA.AgileFramework.Pandora.IOCReplace
{
    /// <summary>
    /// 这个因为映射关系未能全部转换，依赖创建会失败
    /// </summary>
    public class TurboAServiceProvider : IServiceProvider
    {
        private ITurboAContainer _Container = null;

        public TurboAServiceProvider(ITurboAContainer container)
        {
            this._Container = container;
        }
        public object GetService(Type serviceType)
        {
            try
            {
                return this._Container.Resolve(serviceType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            //return this._Container.Resolve(serviceType);
        }
    }



    ///// <summary>
    ///// 这个是写死了，这5个类型需要Builder注册，然后就可以自动生成了
    ///// 其他的还是靠默认容器。。。
    ///// </summary>
    //public class TurboAServiceProvider : IServiceProvider
    //{
    //    private ITurboAContainer _Container = null;
    //    private IServiceCollection _IServiceCollection = null;

    //    public TurboAServiceProvider(ITurboAContainer container, IServiceCollection services)
    //    {
    //        this._Container = container;
    //        this._IServiceCollection = services;
    //    }

    //    public object GetService(Type serviceType)
    //    {
    //        try
    //        {
    //            string[] nameArray = new string[5] {
    //            "TurboA.NET6.Interface.ITestServiceA",
    //            "TurboA.NET6.Interface.ITestServiceB",
    //            "TurboA.NET6.Interface.ITestServiceC",
    //            "TurboA.NET6.Interface.ITestServiceD",
    //            "TurboA.NET6.Interface.ITestServiceE",
    //            };
    //            if (nameArray.Contains(serviceType.FullName))
    //            {
    //                Console.WriteLine($"将{serviceType.FullName}的实例是由自定义容器实例化");
    //                return this._Container.Resolve(serviceType);
    //            }
    //            else
    //            {
    //                Console.WriteLine($"将{serviceType.FullName}的实例是由默认容器实例化");
    //                return _IServiceCollection.BuildServiceProvider().GetService(serviceType);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //return this._IServiceCollection.BuildServiceProvider().GetService(serviceType);
    //            Console.WriteLine(ex.Message);
    //            throw;
    //        }

    //        //return this._Container.Resolve(serviceType);
    //    }
    //}
}
