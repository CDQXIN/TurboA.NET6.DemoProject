using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TurboA.AgileFramework.Pandora.CustomContainer;

namespace TurboA.AgileFramework.Pandora.IOCReplace
{

    /// <summary>
    /// IServiceProviderFactory  实现  IServiceProviderFactory<TurboAContainerBuilder>
    /// 
    /// TurboAContainerBuilder--->IServiceProvider
    /// 欠缺工厂注册-实例注册-泛型处理等
    /// </summary>
    public class TurboAContainerFactory : IServiceProviderFactory<TurboAContainerBuilder>
    {
        /// <summary>
        /// 获取TurboAContainerBuilder
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public TurboAContainerBuilder CreateBuilder(IServiceCollection services)
        {
            return new TurboAContainerBuilder(services);//给TurboAContainerBuilder提供的是IServiceCollection
        }
        /// <summary>
        /// 创建容器的实例，最终就是这个实例为准，全局使用
        /// 我只有个TurboAContainerBuilder的参数，那么创建IServiceProvider的职责就交给你了
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(TurboAContainerBuilder containerBuilder)
        {
            return containerBuilder.GetServiceProvider();
        }
    }

 

   
}
