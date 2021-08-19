using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Zhaoxi.AgileFramework.Pandora.CustomContainer;

namespace Zhaoxi.AgileFramework.Pandora.IOCReplace
{

    /// <summary>
    /// IServiceProviderFactory  实现  IServiceProviderFactory<ZhaoxiContainerBuilder>
    /// 
    /// ZhaoxiContainerBuilder--->IServiceProvider
    /// 欠缺工厂注册-实例注册-泛型处理等
    /// </summary>
    public class ZhaoxiContainerFactory : IServiceProviderFactory<ZhaoxiContainerBuilder>
    {
        /// <summary>
        /// 获取ZhaoxiContainerBuilder
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public ZhaoxiContainerBuilder CreateBuilder(IServiceCollection services)
        {
            return new ZhaoxiContainerBuilder(services);//给ZhaoxiContainerBuilder提供的是IServiceCollection
        }
        /// <summary>
        /// 创建容器的实例，最终就是这个实例为准，全局使用
        /// 我只有个ZhaoxiContainerBuilder的参数，那么创建IServiceProvider的职责就交给你了
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(ZhaoxiContainerBuilder containerBuilder)
        {
            return containerBuilder.GetServiceProvider();
        }
    }

 

   
}
